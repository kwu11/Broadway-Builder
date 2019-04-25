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

namespace BroadwayBuilder.Api.Controllers
{
    [RoutePrefix("production")]
    public class ProductionController : ApiController
    {
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

                //return Created(insert path);
                //return Created("C:\\Users\\ProductionPrograms");
                return Ok("Pdf Uploaded");

            }
            catch (Exception e) {
                // Todo: add proper error handling
                // Todo: log error
                return BadRequest(e.Message);

            }
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult CreateProduction([FromBody] Production production)
        {

            using (var dbcontext = new BroadwayBuilderContext())
            {
                var productionService = new ProductionService(dbcontext);

                try
                {
                    productionService.CreateProduction(production);
                    dbcontext.SaveChanges();

                    // Todo: Turn this into Created(201) once the get endpoint is done so we can return the url to get the item that was just created
                    return Ok("production created");

                }
                // Todo: add proper error handling
                catch (Exception e)
                {
                    return BadRequest();
                }

            }

        }

        [Route("{productionId}")]
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

        [Route("getProductions")]
        [HttpGet]
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

        // Todo: Possibly make them send you the production id
        [Route("update")]
        [HttpPut]
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

                    // Add logging
                }
            }

        }

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
                // Hack: Need to add proper exception handling
                catch (Exception e)
                {
                    return BadRequest();
                }
            }
        }

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

                // Todo: Check if length of httpRequest.Files <= 10 to ensure only 10 photos is uploaded

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

                // Used for loop since foreach did not handle cycling through multiple files well
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

        [Route("{productionId}/create")]
        [HttpPost]
        public IHttpActionResult createProductionDateTime(int productionId, [FromBody] ProductionDateTime productionDateTime )
        {
            try
            {
                using(var dbcontext = new BroadwayBuilderContext())
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

                        // Todo: Change this to a 201 Created(insert url of resource) once get productiondate time route is created
                        return Ok(productionDateTime);
                    }
                    catch (Exception e)
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e )
            {
                return BadRequest("Something went wrong!");
            }

        }

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
