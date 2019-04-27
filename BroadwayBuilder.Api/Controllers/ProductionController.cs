using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using ServiceLayer;
using DataAccessLayer;
using ServiceLayer.Services;
using BroadwayBuilder.Api.Models;
using System.Text;
using System.ComponentModel;
using Swashbuckle.Swagger.Annotations;

namespace BroadwayBuilder.Api.Controllers
{
    [RoutePrefix("production")]
    public class ProductionController : ApiController
    {
        /// <summary>
        /// Sends file to be uploaded to the server filesystem.
        /// </summary>
        /// <remarks>
        /// Before sending file to be uploaded it validates the file for 
        /// valid extension, valid file size, and valid file amount.
        /// </remarks>
        /// <param name="productionId"> The unique production's ID is used to upload that file to the corresponding production.</param>
        [SwaggerResponse(HttpStatusCode.OK, "A message stating if pdf was uploaded succesfully or an error message if something went wrong.")]
        [Route("{productionId}/uploadProgram")]
        [HttpPut]
        public IHttpActionResult UploadProductionProgram(int productionId)
        {
            var dbcontext = new BroadwayBuilderContext();
            var productionService = new ProductionService(dbcontext);

            // Try to upload pdf and save to server filesystem
            try
            {
                // Get the full request of the current http request
                var httpRequest = HttpContext.Current.Request;
                var fileCollection = httpRequest.Files;

                var fileValidator = new FileValidator();

                int MaxContentLength = 1024 * 1024 * 1;
                int maxFileCount = 1;
                var extensions = new List<string>() { ".pdf" };

                // Validate files for valid extension, and size
                var validationResult = fileValidator.ValidateFiles(fileCollection, extensions, MaxContentLength, maxFileCount);

                if (!validationResult.ValidationSuccessful)
                {
                    var errorMessage = string.Join("\n", validationResult.Reasons);
                    return BadRequest(errorMessage);
                }

                // Send file to up uploaded to server
                foreach (string filename in fileCollection)
                {
                    var putFile = fileCollection[filename];

                 // Todo: check if id is null or not then proceed
                 productionService.UploadProgram(productionId, putFile);
                }

                return Ok("Pdf Uploaded");

            }
            catch (Exception e) {
                // Todo: add proper error handling in production service
                // Todo: log error
                return BadRequest(e.Message);

            }
        }

        /// <summary>
        /// Creates a production.
        /// </summary>
        /// <param name="production"> The production object</param>
        [Route("create")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, "The production created and its url.", typeof(Production))]
        public IHttpActionResult CreateProduction([FromBody] Production production)
        {

            using (var dbcontext = new BroadwayBuilderContext())
            {
                var productionService = new ProductionService(dbcontext);

                try
                {
                    productionService.CreateProduction(production);
                    dbcontext.SaveChanges();

                    var productionUrl = Url.Link("GetProductionById", new { productionId = production.ProductionID });

                    // Todo: Turn this into Created(201) once the get endpoint is done so we can return the url to get the item that was just created
                    return Created(productionUrl, production);
                }
                // Todo: add proper error handling
                catch (Exception e)
                {
                    return BadRequest();
                }

            }

        }

        /// <summary>
        /// Gets a production by its ID.
        /// </summary>
        /// <param name="productionId">The unique production ID</param>
        [SwaggerResponse(HttpStatusCode.OK, "The production object that matches the ID.", typeof(Production))]
        [Route("{productionId}", Name = "GetProductionById")]
        [HttpGet]
        public IHttpActionResult GetProductionById(int productionId)
        {
            using (var dbcontext = new BroadwayBuilderContext())

            {
                var productionService = new ProductionService(dbcontext);

                try
                {
                    Production current_production = productionService.GetProduction(productionId);

                    return Ok(current_production);
                }
                // Hack: Need to add proper exception handling
                catch (Exception e)
                {
                    return BadRequest();
                }
            }
        }

        /// <summary>
        /// Gets all productions before a previous date or after a current date.
        /// </summary>
        /// <remarks>
        /// Both dates cannot be null. Either a previous date or current date must be provided.
        /// In addition, if a theater ID is sent it will return productions that correspond only to that theater ID.
        /// The default page number is set to 1. The default page size is set to 10.
        /// </remarks>
        /// <param name="currentDate">Optional: The current date.</param>
        /// <param name="previousDate">Optional: Any previous date.</param>
        /// <param name="theaterID">Optional: The unique thater ID.</param>
        /// <param name="pageNum">The specific page wanted.</param>
        /// <param name="pageSize">The amount of production objects wanted per page.</param>
        [Route("getProductions")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, "A list of Productions.", typeof(List<ProductionResponseModel>))]
        public IHttpActionResult GetProductions(DateTime? currentDate = null, DateTime? previousDate = null, int? theaterID = null, int pageNum = 1, int pageSize = 10)
        {
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var productionService = new ProductionService(dbcontext);

                    try
                    {
                        if (previousDate != null) {

                            var productionResponses = productionService.GetProductionsByPreviousDate((DateTime)previousDate, theaterID, pageNum, pageSize)
                                .Select(production => new ProductionResponseModel()
                                {
                                    DirectorFirstName = production.DirectorFirstName,
                                    DirectorLastName = production.DirectorLastName,
                                    ProductionID = production.ProductionID,
                                    ProductionName = production.ProductionName,
                                    StateProvince = production.StateProvince,
                                    Street = production.Street,
                                    TheaterID = production.TheaterID,
                                    Zipcode = production.Zipcode,
                                    City = production.City,
                                    Country = production.Country,
                                    DateTimes = production.ProductionDateTime.Select(datetime => new ProductionDateTimeResponseModel()
                                    {
                                        Date = datetime.Date,
                                        ProductionDateTimeId = datetime.ProductionDateTimeId,
                                        Time = datetime.Time
                                    }).ToList()
                                }).ToList();

                            return Ok(productionResponses);
                        }
                        else if (currentDate != null)
                        {
                            var productionResponses = productionService.GetProductionsByCurrentAndFutureDate((DateTime)currentDate, theaterID, pageNum, pageSize)
                                .Select(production => new ProductionResponseModel()
                                {
                                    City = production.City,
                                    DirectorFirstName = production.DirectorFirstName,
                                    DirectorLastName = production.DirectorLastName,
                                    Country = production.Country,
                                    ProductionID = production.ProductionID,
                                    ProductionName = production.ProductionName,
                                    StateProvince = production.StateProvince,
                                    Street = production.Street,
                                    TheaterID = production.TheaterID,
                                    Zipcode = production.Zipcode,
                                    DateTimes = production.ProductionDateTime.Select(datetime => new ProductionDateTimeResponseModel()
                                    {
                                        Date = datetime.Date,
                                        ProductionDateTimeId = datetime.ProductionDateTimeId,
                                        Time = datetime.Time
                                    }).ToList()
                                }).ToList();

                            return Ok(productionResponses);
                        }

                        // none of the if conditions were met therfore...
                        return BadRequest("PreviousDate and Current date were both null");
                    }
                    // Todo: Add proper exception handling for getting a production
                    catch (Exception e)
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e) // Todo: Catch a SqlException ... or Sqlconnection exception?
            {
                return BadRequest("Something big went bad!");
            }
        }

        /// <summary>
        /// Updates a production object that matches in the database.
        /// </summary>
        /// <param name="production_to_update"> The production object to update.</param>
        [Route("update")]
        [HttpPut]
        [SwaggerResponse(HttpStatusCode.OK, "The updated production object.", typeof(Production))]
        public IHttpActionResult UpdateProduction([FromBody] Production production_to_update)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                var productionService = new ProductionService(dbcontext);

                try
                {
                    if (production_to_update == null)
                    {
                        return BadRequest("no production object provided");
                    }
                    else if (production_to_update.ProductionID == 0)
                    {
                        return BadRequest("Production id was not sent");
                    }


                    Production updated_production = productionService.UpdateProduction(production_to_update);
                    dbcontext.SaveChanges();

                    return Ok(updated_production);

                }
                // Catching all exceptions and returning to user
                catch (Exception e)
                {
                    return BadRequest(e.Message);

                    // Todo: Log error
                }
            }

        }

        /// <summary>
        /// Deletes the production object that matches the ID from the database.
        /// </summary>
        /// <param name="productionid">The unique production ID.</param>
        [SwaggerResponse(HttpStatusCode.OK, "A string status")]
        [Route("delete/{productionid}")]
        [HttpDelete]
        public IHttpActionResult deleteProduction(int productionid)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                var productionService = new ProductionService(dbcontext);

                try
                {
                    productionService.DeleteProduction(productionid);
                    dbcontext.SaveChanges();

                    return Ok("Production deleted succesfully");

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);

                    // Todo: Log error
                }
            }
        }

        /// <summary>
        /// Sends each file to be saved onto the server's file system.
        /// </summary>
        /// <remarks>
        /// Before sending file to be uploaded it validates the file for 
        /// valid extension, valid file size, and valid file amount.
        /// </remarks>
        /// <param name="productionId">The unique productions ID. Used to relate photos to the corresponding production.</param>
        [SwaggerResponse(HttpStatusCode.OK, "A string status message.")]
        [Route("{productionId}/uploadPhoto")]
        [HttpPost]
        public IHttpActionResult uploadPhoto(int productionId)
        {
            var dbcontext = new BroadwayBuilderContext();
            var productionService = new ProductionService(dbcontext);

            //try to upload pdf and save to server filesystem
            try
            {
                //get the content, headers, etc the full request of the current http request
                var httpRequest = HttpContext.Current.Request;
                var fileCollection = httpRequest.Files;

                var fileValidator = new FileValidator();

                int MaxContentLength = 1 * 1024 * 1024 * 5; //Size = 5 MB
                var validExtensions = new List<string>() {".jpg"};
                int maxFileCount = 5;

                var validationResult = fileValidator.ValidateFiles(fileCollection, validExtensions, MaxContentLength, maxFileCount);

                if (!validationResult.ValidationSuccessful)
                {
                    var errorMessage = string.Join("\n", validationResult.Reasons);
                    return BadRequest(errorMessage);
                }

                var count = 0;

                // Used for loop since foreach did not handle cycling through multiple files well.
                for (int i= 0; i < httpRequest.Files.Count; i++)
                {
                    // Grab current file of the request
                    var putFile = httpRequest.Files[i];
                    
                   // Send to production service where functinality to save the file is                        
                   productionService.UploadPhoto(productionId, count, putFile);
                    
                    count++;
                }

                return Ok("Photo Uploaded");
            }

            catch (Exception ex)
            {
                // Todo: add proper error handling
                // Todo: log error
                return BadRequest(ex.Message);

            }
        }

        /// <summary>
        /// Gets a list of photos urls pertaining to the specified production ID.
        /// </summary>
        /// <param name="productionId">The unique production ID.</param>
        [SwaggerResponse(HttpStatusCode.OK, "A list of file urls", typeof(List<string>))]
        [Route("{productionId}/getPhotos")]
        [HttpGet]
        public IHttpActionResult getPhotos(int productionId)
        {
            
            // Virtual Directory path
            var filepath = HostingEnvironment.MapPath("~/Photos/Production" + productionId);

            // Grabbing information about the directory at this path. Todo: Look into changing to using Directory rather than DirectoryInfo
            DirectoryInfo dir = new DirectoryInfo(filepath);

            FileInfo[] filepaths = dir.GetFiles();

            var filenames = new List<string>();
            // Grab each files name and put it into a list 
            foreach (FileInfo fileTemp in filepaths)
            {
                filenames.Add(fileTemp.Name);
            }

            var fileUrls = new List<string>();
            // Give each file name their approriate url in order to get photos
            foreach (var fi in filenames)
            {
                fileUrls.Add("https://api.broadwaybuilder.xyz/Photos/Production" + productionId + "/" + fi);
            }
        
            return Ok(fileUrls);
        }

        ///// <summary>
        ///// Creates an object pertaining the a specific date and time for the production matching the production ID.
        ///// </summary>
        ///// <param name="productionId">The unique production ID.</param>
        ///// <param name="productionDateTime">The production Date Time object that will be attached to the production matching the ID.</param>
        ///// <returns>The production Date time object that was succesfully created.</returns>
        [SwaggerResponse(HttpStatusCode.OK, "The production date time created and its url.", typeof(ProductionDateTime))]
        [Route("{productionId}/create")]
        [HttpPost]
        public IHttpActionResult createProductionDateTime(int productionId, [FromBody] ProductionDateTime productionDateTime)
        {
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var productionService = new ProductionService(dbcontext);

                    try
                    {
                        if (productionDateTime == null)
                        {
                            return BadRequest("no production date time object provided");
                        }

                        productionDateTime.ProductionID = productionId;
                        productionService.CreateProductionDateTime(productionDateTime);
                        dbcontext.SaveChanges();

                        var productionDateTimeCreatedUrl = Url.Link("GetProductionDateTimeByID", new { productionDateTimeID = productionDateTime.ProductionDateTimeId });

                        // Todo: Change this to a 201 Created(insert url of resource) once get productiondate time route is created
                        return Created(productionDateTimeCreatedUrl, productionDateTime);
                    }
                    catch (Exception e)
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong!");
            }

        }

        /// <summary>
        /// Updates either the date or time of a Production Date Time object that matches the production date time ID.
        /// </summary>
        /// <param name="productionDateTimeId">The unique production date time ID.</param>
        /// <param name="productionDateTime">The date and time attribute of the object.</param>
        [SwaggerResponse(HttpStatusCode.OK, "The updated production date time", typeof(ProductionDateTime))]
        [Route("{productionDateTimeID}")]
        [HttpPut]
        public IHttpActionResult updateProductionDateTime(int productionDateTimeId, [FromBody] ProductionDateTime productionDateTime)
        {
            try
            {
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var productionService = new ProductionService(dbContext);

                    try
                    {
                        if (productionDateTime == null)
                        {
                            return BadRequest("no date time was provided");
                        }

                        // Set the production date time id that is to be updated to the id in the uri
                        productionDateTime.ProductionDateTimeId = productionDateTimeId;
                       var updatedProductionDateTime = productionService.UpdateProductionDateTime(productionDateTime);
                        dbContext.SaveChanges();

                        return Ok(updatedProductionDateTime);
                    }
                    catch (Exception e)
                    {
                        // If none of those if statements were met and we couldnt update a production...
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                // Todo: add proper error response when requet fails due to not being able to create dbcontext...
                return BadRequest("Something bad happend!");
            }
        }

        /// <summary>
        /// Delete a production date time object that corresponds to the ID.
        /// </summary>
        /// <param name="productionDateTimeid">The unique production date time ID.</param>
        /// <param name="productionDateTimeToDelete">The production date time object to delete.</param>
        [SwaggerResponse(HttpStatusCode.OK, "A string status")]
        [Route("{productionDateTimeId}")]
        [HttpDelete]
        public IHttpActionResult deleteProductiondateTime(int productionDateTimeid, ProductionDateTime productionDateTimeToDelete)
        {
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var productionService = new ProductionService(dbcontext);

                    try
                    {
                        if (productionDateTimeToDelete == null)
                        {
                            return BadRequest("no production date time object provided");
                        }

                        productionService.DeleteProductionDateTime(productionDateTimeToDelete);
                        dbcontext.SaveChanges();
                        return Ok("Production date time deleted succesfully!");

                    }
                    catch (Exception e)
                    {
                        // Todo: Catch a specific error so you can tell send a specific response model stating why a production date time was not able to be deleted
                        return BadRequest("Was not able to delete production date time because.....");
                    }
                }
            }
            catch (Exception e)
            {
                // Todo: User proper error handling responses catch  a more specific error
                return BadRequest("Major error happened!"); 
            }
        }
    }
}
