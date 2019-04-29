using System;
using DataAccessLayer;
using BroadwayBuilder.Api.Controllers;
using BroadwayBuilder.Api.Models;
using System.Web.Http.Results;
using ServiceLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Web.Http.SelfHost;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.IO;
using System.Net.Http.Formatting;
using TestUtilites;
using System.Configuration;

namespace BroadwayBuilder.Api.Tests
{
    [TestClass]
    public class ProductionControllerTest
    {
        /*Info:
         *      Am not able to test. Cannot send an HttpContext.Current.Requset
         * */
        //[TestMethod]
        //public async Task ProductionController_UploadProgram_Fail()
        //{
        //    // Arrange
        //    var dbcontext = new BroadwayBuilderContext();
        //    var theaterService = new TheaterService(dbcontext);

        //    var theater = new Theater()
        //    {
        //        TheaterName = "Some Theater 1",
        //        StreetAddress = "Theater St",
        //        State = "CA",
        //        City = "LA",
        //        CompanyName = "Regal",
        //        Country = "US",
        //        PhoneNumber = "123456789"
        //    };

        //    theaterService.CreateTheater(theater);
        //    dbcontext.SaveChanges();

        //    var production = new Production()
        //    {
        //        ProductionName = "The Lion King 2",
        //        DirectorFirstName = "Jane",
        //        DirectorLastName = "Doe",
        //        Street = "Anahiem",
        //        City = "Long Beach",
        //        StateProvince = "California",
        //        Country = "United States",
        //        Zipcode = "919293",
        //        TheaterID = theater.TheaterID
        //    };

        //    var productionService = new ProductionService(dbcontext);
        //    productionService.CreateProduction(production);
        //    dbcontext.SaveChanges();

        //    var baseAddress = new Uri("http://localhost:8000/");
        //    var config = new HttpSelfHostConfiguration(baseAddress);

        //    WebApiConfig.Register(config);

        //    var server = new HttpSelfHostServer(config);
        //    var client = new HttpClient(server)
        //    {
        //        BaseAddress = baseAddress
        //    };

        //    var productionController = new ProductionController();

        //    // Act
        //    var fileUploadPdf = "test";

        //    var content = new ByteArrayContent(Encoding.UTF8.GetBytes(fileUploadPdf));
        //    content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        //    {
        //        Name = "file",
        //        FileName = "upload.pdf"
        //    };


        //    var response = await client.PutAsync("http://api.broadwaybuilder.xyz", new MultipartFormDataContent("----Boundry")
        //    {
        //        content
        //    });

        //    //var request = new HttpRequest("", "http://localhost:8000/production/1/uploadprogram", "");

        //    HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost:8000/production/1/uploadprogram", null), new HttpResponse(null));
        //   // request.ContentType = "application/pdf";
        //   // HttpContext.Current = new HttpContext(request, new HttpResponse(new StringWriter()));

            

        //    productionService.DeleteProduction(production.ProductionID);
        //    dbcontext.SaveChanges();
        //    theaterService.DeleteTheater(theater);
        //    dbcontext.SaveChanges();

        //    // Assert
        //    Assert.IsNotNull(response);
        //    Assert.AreEqual("Pdf Uploaded", response.Content);
        //}

        [TestMethod]
        public async Task ProductionController_CreateProduction_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "Some Theater 1",
                StreetAddress = "Theater St",
                State = "CA",
                City = "LA",
                CompanyName = "Regal",
                Country = "US",
                PhoneNumber = "123456789"
            };

            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var production = new Production()
            {
                ProductionName = "The Lion King 2",
                DirectorFirstName = "Jane",
                DirectorLastName = "Doe",
                Street = "Anahiem",
                City = "Long Beach",
                StateProvince = "California",
                Country = "United States",
                Zipcode = "919293",
                TheaterID = theater.TheaterID
            };

            var productionService = new ProductionService(dbcontext);

           /*
            * Info:
            *     Using Self Hosting of api in order to make a request
            * 
            */

            var baseAddress = new Uri("http://localhost:8000/");
            var config = new HttpSelfHostConfiguration(baseAddress);

            WebApiConfig.Register(config);

            var server = new HttpSelfHostServer(config);
            var client = new HttpClient(server)
            {
                BaseAddress = baseAddress
            };

            // Act
            var response = await client.PostAsync<Production>("production/create", production, new JsonMediaTypeFormatter());

            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();

            // Assert
            Assert.IsNotNull(response);

            var success = response.TryGetContentValue(out ProductionResponseModel productionFromResponse);
            Assert.IsTrue(success);

            //Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(productionFromResponse);

        }

        [TestMethod]
        public void ProductionController_GetProductionById_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "Some Theater",
                StreetAddress = "Theater St",
                State = "CA",
                City = "LA",
                CompanyName = "Regal",
                Country = "US",
                PhoneNumber = "123456789"
            };

            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionService = new ProductionService(dbcontext);

            var production = new Production
            {
                ProductionName = "The Pajama Game",
                DirectorFirstName = "Doris",
                DirectorLastName = "Day",
                City = "San Diego",
                StateProvince = "California",
                Country = "U.S",
                TheaterID = theater.TheaterID,
                Street = "123 Sesame St",
                Zipcode = "91911"
            };

            productionService.CreateProduction(production);
            dbcontext.SaveChanges();

            var productionController = new ProductionController();

            // Act
           var actionResult = productionController.GetProductionById(production.ProductionID);

            var response = actionResult as OkNegotiatedContentResult<Production>;


            productionService.DeleteProduction(response.Content.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
           
            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual(production.ProductionID, response.Content.ProductionID);

        }

        [TestMethod]
        public void ProductionController_GetProductions_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "Some Theater",
                StreetAddress = "Theater St",
                State = "CA",
                City = "LA",
                CompanyName = "Regal",
                Country = "US",
                PhoneNumber = "123456789"
            };

            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();


            var productionService = new ProductionService(dbcontext);

            var production1 = new Production()
            {
                ProductionName = "The Lion King 14",
                DirectorFirstName = "Joan",
                DirectorLastName = "Doe",
                Street = "123 Anahiem St",
                City = "Long Beach",
                StateProvince = "California",
                Country = "United States",
                Zipcode = "919293",
                TheaterID = theater.TheaterID
            };


            productionService.CreateProduction(production1);
            dbcontext.SaveChanges();

            var production2 = new Production()
            {
                ProductionName = "The Lion King 15",
                DirectorFirstName = "Joan",
                DirectorLastName = "Doe",
                Street = "123 Anahiem St",
                City = "Long Beach",
                StateProvince = "California",
                Country = "United States",
                Zipcode = "919293",
                TheaterID = theater.TheaterID
            };

            productionService.CreateProduction(production2);
            dbcontext.SaveChanges();

            var productionDateTime1 = new ProductionDateTime()
            {
                Date = DateTime.Parse("3/23/2019 3:22:29 PM"),
                Time = TimeSpan.Parse("10:30:00"),
                ProductionID = production1.ProductionID
            };

            productionService.CreateProductionDateTime(productionDateTime1);
            dbcontext.SaveChanges();

            var productionDateTime2 = new ProductionDateTime()
            {
                Date = DateTime.Parse("3/29/2019 3:22:29 PM"),
                Time = TimeSpan.Parse("5:30:00"),
                ProductionID = production2.ProductionID
            };

            productionService.CreateProductionDateTime(productionDateTime2);
            dbcontext.SaveChanges();


            var productionController = new ProductionController();

            // Act
            var actionResult = productionController.GetProductions(currentDate: new DateTime(2019, 3, 1));

            var response = actionResult as OkNegotiatedContentResult<List<ProductionResponseModel>>;

            var productions = response.Content;

            var expected = true;
            var actual = false;

            if (productions.Count > 0)
            {
                actual = true;
            }

            productionService.DeleteProductionDateTime(productionDateTime2);
            dbcontext.SaveChanges();
            productionService.DeleteProductionDateTime(productionDateTime1);
            dbcontext.SaveChanges();
            productionService.DeleteProduction(production1.ProductionID);
            dbcontext.SaveChanges();
            productionService.DeleteProduction(production2.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();

            // Assert
            Assert.IsNotNull(response.Content);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void ProductionController_UpdateProduction_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "The Magicians",
                StreetAddress = "Pantene",
                State = "CA",
                City = "LA",
                CompanyName = "123 Sesame St",
                Country = "US",
                PhoneNumber = "123456789"
            };

            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();


            var productionService = new ProductionService(dbcontext);

            var production = new Production()
            {
                ProductionName = "The Lion King",
                DirectorFirstName = "Jane",
                DirectorLastName = "Doe",
                Street = "Anahiem",
                City = "Long Beach",
                StateProvince = "California",
                Country = "United States",
                Zipcode = "919293",
                TheaterID = theater.TheaterID
            };

            production.ProductionName = "The Updated Lion King";
            production.StateProvince = "Utah";
            production.DirectorLastName = "Mangos";

            productionService.CreateProduction(production);
            dbcontext.SaveChanges();

            var productionController = new ProductionController();

            // Act

            var actionResult = productionController.UpdateProduction(production);

            var response = actionResult as OkNegotiatedContentResult<Production>;
            var updatedProduction = response.Content;

            productionService.DeleteProduction(response.Content.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(production.ProductionID, updatedProduction.ProductionID);
            Assert.AreEqual("The Updated Lion King", updatedProduction.ProductionName);
            Assert.AreEqual("Utah", updatedProduction.StateProvince);
            Assert.AreEqual("Mangos", updatedProduction.DirectorLastName);
            
        }

        [TestMethod]
        public void ProductionController_DeleteProduction_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "Some Theater",
                StreetAddress = "Theater St",
                State = "CA",
                City = "LA",
                CompanyName = "Regal",
                Country = "US",
                PhoneNumber = "123456789"
            };

            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionService = new ProductionService(dbcontext);

            var production = new Production
            {
                ProductionName = "The Pajama Game",
                DirectorFirstName = "Doris",
                DirectorLastName = "Day",
                City = "San Diego",
                StateProvince = "California",
                Country = "U.S",
                TheaterID = theater.TheaterID,
                Street = "123 Sesame St",
                Zipcode = "91911"
            };

            productionService.CreateProduction(production);
            dbcontext.SaveChanges();

            var productionController = new ProductionController();

            // Act
            var actionResult = productionController.deleteProduction(production.ProductionID);

            var response = actionResult as OkNegotiatedContentResult<string>;

            var dbcontext_ = new BroadwayBuilderContext();
            var theaterService_ = new TheaterService(dbcontext_);
            var theater_ = theaterService.GetTheaterByID(theater.TheaterID);
            theaterService_.DeleteTheater(theater_);
            dbcontext_.SaveChanges();
           
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("Production deleted succesfully", response.Content);

        }

        [TestMethod]
        public void ProductionController_CreateProductionDateTime_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "Some Theater 1",
                StreetAddress = "Theater St",
                State = "CA",
                City = "LA",
                CompanyName = "Regal",
                Country = "US",
                PhoneNumber = "123456789"
            };

            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var production = new Production()
            {
                ProductionName = "The Lion King 2",
                DirectorFirstName = "Jane",
                DirectorLastName = "Doe",
                Street = "Anahiem",
                City = "Long Beach",
                StateProvince = "California",
                Country = "United States",
                Zipcode = "919293",
                TheaterID = theater.TheaterID
            };

            var productionService = new ProductionService(dbcontext);
            productionService.CreateProduction(production);
            dbcontext.SaveChanges();

            var date = DateTime.Parse("3/23/2019 3:22:29 PM");
            var time = TimeSpan.Parse("10:30:00");

            var productionDateTime = new ProductionDateTime(production.ProductionID, date, time);

            var productionController = new ProductionController();
            
            // Act
            var actionResult = productionController.createProductionDateTime(production.ProductionID, productionDateTime);
            var response = actionResult as OkNegotiatedContentResult<string>;

            productionService.DeleteProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("Production Date and time have been added!", response.Content);
        }

        [TestMethod]
        public void ProductionController_UpdateProductionDateTime_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "The Magicians",
                StreetAddress = "Pantene",
                State = "CA",
                City = "LA",
                CompanyName = "123 Sesame St",
                Country = "US",
                PhoneNumber = "123456789"
            };

            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionService = new ProductionService(dbcontext);

            var production = new Production
            {
                ProductionName = "The Pajama Game 1",
                DirectorFirstName = "Doris",
                DirectorLastName = "Day",
                City = "San Diego",
                StateProvince = "California",
                Country = "U.S",
                TheaterID = theater.TheaterID,
                Street = "1234 Sesame St",
                Zipcode = "91911"
            };

            productionService.CreateProduction(production);
            dbcontext.SaveChanges();

            var date = DateTime.Parse("3/28/2019 3:22:29 PM");
            var time = TimeSpan.Parse("11:30:00");

            var productionDateTime = new ProductionDateTime(production.ProductionID, date, time);
            productionService.CreateProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();

            productionDateTime.Date = DateTime.Parse("3/27/2019 3:22:29 PM");
            productionDateTime.Time = TimeSpan.Parse("9:30:00");

            var productionController = new ProductionController();

            // Act
            var actionResult = productionController.updateProductionDateTime(productionDateTime.ProductionDateTimeId, productionDateTime);

            var response = actionResult as OkNegotiatedContentResult<ProductionDateTime>;
            var updatedProductionDateTime = response.Content;

            var expected = new
            {
                DateTime = DateTime.Parse("3/27/2019 3:22:29 PM"),
                TimeSpan = TimeSpan.Parse("9:30:00")
            };

            var actual = updatedProductionDateTime;

            productionService.DeleteProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();
            productionService.DeleteProduction(response.Content.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(productionDateTime.ProductionDateTimeId, updatedProductionDateTime.ProductionDateTimeId);
            Assert.AreEqual(expected.DateTime, actual.Date);
            Assert.AreEqual(expected.TimeSpan, actual.Time);
        }

        [TestMethod]
        public void ProductionController_DeleteProductionDateTime_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "The Magicians",
                StreetAddress = "Pantene",
                State = "CA",
                City = "LA",
                CompanyName = "123 Sesame St",
                Country = "US",
                PhoneNumber = "123456789"
            };

            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionService = new ProductionService(dbcontext);

            var production = new Production
            {
                ProductionName = "The Pajama Game 1",
                DirectorFirstName = "Doris",
                DirectorLastName = "Day",
                City = "San Diego",
                StateProvince = "California",
                Country = "U.S",
                TheaterID = theater.TheaterID,
                Street = "1234 Sesame St",
                Zipcode = "91911"
            };

            productionService.CreateProduction(production);
            dbcontext.SaveChanges();

            var date = DateTime.Parse("3/28/2019 3:22:29 PM");
            var time = TimeSpan.Parse("11:30:00");

            var productionDateTime = new ProductionDateTime(production.ProductionID, date, time);

            productionService.CreateProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();

            var productionController = new ProductionController();

            // Act
            var actionResult = productionController.deleteProductiondateTime(productionDateTime.ProductionDateTimeId, productionDateTime);
            var response = actionResult as OkNegotiatedContentResult<string>;

            var dbcontext_ = new BroadwayBuilderContext();
            var theaterService_ = new TheaterService(dbcontext_);
            var theater_ = theaterService.GetTheaterByID(theater.TheaterID);
            theaterService_.DeleteTheater(theater_);
            dbcontext_.SaveChanges();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("Production date time deleted succesfully!", response.Content);
        }

        [TestMethod]
        public void ProductionController_GetPhotos_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "Some Theater",
                StreetAddress = "Theater St",
                State = "CA",
                City = "LA",
                CompanyName = "Regal",
                Country = "US",
                PhoneNumber = "123456789"
            };

            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionService = new ProductionService(dbcontext);

            var production = new Production
            {
                ProductionName = "The Pajama Game",
                DirectorFirstName = "Doris",
                DirectorLastName = "Day",
                City = "San Diego",
                StateProvince = "California",
                Country = "U.S",
                TheaterID = theater.TheaterID,
                Street = "123 Sesame St",
                Zipcode = "91911"
            };

            productionService.CreateProduction(production);
            dbcontext.SaveChanges();

            var productionController = new ProductionController();

            var mockPostedPdfFile1 = new MockPostedFile("jpg", 5000000, "1.jpg");
            var mockPostedPdfFile2 = new MockPostedFile("jpg", 5000000, "2.jpg");

            productionService.SavePhoto(production.ProductionID, mockPostedPdfFile1);
            productionService.SavePhoto(production.ProductionID, mockPostedPdfFile2);

            var currentDirectory = ConfigurationManager.AppSettings["FileDir"];

            var dir = Path.Combine(currentDirectory, "Photos/");
            var subdir = Path.Combine(dir, $"Production{production.ProductionID}/");

            // Act
            var actionResult = productionController.getPhotos(production.ProductionID);

            // Need access to contet of the response
            var response = actionResult as OkNegotiatedContentResult<List<string>>;

            var photoUrls = response.Content;

            var expected = true;
            var actual = false;

            if (photoUrls.Count == 2)
            {
                actual = true;
            }

            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Directory.Delete(subdir, true);
            
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(expected, actual);          
        }

 
    }

}
