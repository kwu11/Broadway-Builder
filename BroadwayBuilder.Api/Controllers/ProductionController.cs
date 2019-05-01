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
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

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

                // Send file to be saved to server
                foreach (string filename in fileCollection)
                {
                    HttpPostedFileBase putFile = new HttpPostedFileWrapper(fileCollection[filename]);


                    productionService.SaveProgram(productionId, putFile);
                }

                return Ok("Pdf Uploaded");

            }
            catch (Exception e) { // Todo: log error
                // Todo: add proper error handling in production service
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
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var productionService = new ProductionService(dbcontext);

                        productionService.CreateProduction(production);
                        dbcontext.SaveChanges();

                    var productionUrl = Url.Link("GetProductionById", new { productionId = production.ProductionID });

                    return Created(productionUrl, new ProductionResponseModel()
                    {
                        ProductionID = production.ProductionID,
                        DirectorFirstName = production.DirectorFirstName,
                        DirectorLastName = production.DirectorLastName,
                        ProductionName = production.ProductionName,
                        Street = production.Street,
                        City = production.City,
                        StateProvince = production.StateProvince,
                        Country = production.Country,
                        TheaterID = production.TheaterID
                    });
                }
            }
            catch (DbUpdateException e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            // Todo: add proper error handling
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())

                {
                    var productionService = new ProductionService(dbcontext);

                        Production current_production = productionService.GetProduction(productionId);

                        return Ok(current_production);
                }
            }
            catch(DbUpdateException e)
            {
                return InternalServerError(e);
            }
            // Catching all exceptions
            catch (Exception e) // Todo: log error
            {
                return BadRequest(e.Message);
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

                        if (previousDate != null || currentDate == null) {

                            var productionResponses = productionService.GetProductionsByPreviousDate((DateTime)previousDate, theaterID,  pageNum, pageSize)
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
                        else 
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

                }
            }
            catch (DbUpdateException e) // Todo: Log Error
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            // Todo: Add proper exception handling for getting a production
            catch (Exception e) // Todo: Log Error Might not catch this since okay with returning [] emtpy list
            {
                return BadRequest();
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
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var productionService = new ProductionService(dbcontext);

                        if (production_to_update == null)
                        {
                            return BadRequest("No production object provided");
                        }
                        else if (production_to_update.ProductionID == 0)
                        {
                            return BadRequest("Production id was not sent");
                        }


                        Production updated_production = productionService.UpdateProduction(production_to_update);
                        dbcontext.SaveChanges();

                        return Ok(updated_production);
                }
            }
            catch (DbUpdateException e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            // Catching all general exceptions and returning to user
            catch (Exception e)  // Todo: Log error
            {
                return BadRequest(e.Message);
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
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var productionService = new ProductionService(dbcontext);

                        productionService.DeleteProduction(productionid);
                        dbcontext.SaveChanges();

                    return Ok("Production deleted succesfully");
                }
            }
            catch (DbUpdateException e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch (Exception e) // Todo: Log error
            {
                return BadRequest(e.Message);
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

                // Used for loop since foreach did not handle cycling through multiple files well.
                for (int i= 0; i < httpRequest.Files.Count; i++)
                {
                    // Grab current file of the request
                    HttpPostedFileBase putFile = new HttpPostedFileWrapper(httpRequest.Files[i]);
                    
                   // Send to production service where functinality to save the file is                        
                   productionService.SavePhoto(productionId, putFile);
                   
                }

                return Ok("Photo Uploaded");
            }

            catch (Exception ex) // Todo: log error
            {
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
        public IHttpActionResult GetPhotos(int productionId)
        {

            var currentDirectory = ConfigurationManager.AppSettings["FileDir"];

            var dir = Path.Combine(currentDirectory, "Photos/");
            var subdir = Path.Combine(dir, $"Production{productionId}/");

            // Grabbing information about the directory at this path.
            DirectoryInfo direc = new DirectoryInfo(subdir);

            FileInfo[] filepaths = direc.GetFiles();

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
        public IHttpActionResult CreateProductionDateTime(int productionId, [FromBody] ProductionDateTime productionDateTime)
        {
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var productionService = new ProductionService(dbcontext);
                   
                        if (productionDateTime == null)
                        {
                            return BadRequest("no production date time object provided");
                        }

                        productionDateTime.ProductionID = productionId;
                        productionService.CreateProductionDateTime(productionId,productionDateTime);
                        dbcontext.SaveChanges();

                        return Ok("Production Date and time have been added!");                  
                }
            }
            catch (DbUpdateException e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
            }
            catch (DbUpdateException e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
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

                    if (productionDateTimeToDelete == null)
                    {
                        return BadRequest("no production date time object provided");
                    }

                    productionService.DeleteProductionDateTime(productionDateTimeToDelete);
                    dbcontext.SaveChanges();
                    return Ok("Production date time deleted succesfully!");
                }
            }
            catch (DbUpdateException e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
