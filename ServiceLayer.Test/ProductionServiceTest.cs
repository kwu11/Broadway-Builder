using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using ServiceLayer.Services;
using System.Collections.Generic;
using ServiceLayer.Exceptions;
using System.IO;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TestUtilites;

namespace ServiceLayer.Test
{
    [TestClass]
    public class ProductionServiceTest
    {
        [TestMethod]
        public void ProductionService_CreateProduction_Pass()
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
           
            var expected = true;
            var actual = false;

            var productionService = new ProductionService(dbcontext);


            // Act
            productionService.CreateProduction(production);
            dbcontext.SaveChanges();

            if (production.ProductionID > 0)
            {
                actual = true;
            }

            // Assert
            var dbcontext_ = new BroadwayBuilderContext();
            var productionService_ = new ProductionService(dbcontext_);

            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Assert.AreEqual(expected, actual);


        }

        [TestMethod]
        public void ProductionService_GetProductionById_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "The Language",
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

            var expected = true;
            var actual = false;

            // Act 
            var readProduction = productionService.GetProduction(production.ProductionID);

            if (readProduction != null)
            {
                actual = true;
            }

            // Assert
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void ProductionService_GetProductionsByPreviousDate_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "The Language",
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

            var production1 = new Production()
            {
                ProductionName = "The Lion King 10",
                DirectorFirstName = "Joan",
                DirectorLastName = "Doe",
                Street = "123 Anahiem St",
                City = "Long Beach",
                StateProvince = "California",
                Country = "United States",
                Zipcode = "919293",
                TheaterID = theater .TheaterID
            };

          
            productionService.CreateProduction(production1);
            dbcontext.SaveChanges();

            var production2 = new Production()
            {
                ProductionName = "The Lion King 11",
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

            var expected = true;
            var actual = false;

            // Act
            var readProductionsList = productionService.GetProductionsByPreviousDate(new DateTime(2019,3,1), null, 1, 10); // Theater id is meant to be null

            if (readProductionsList != null)
            {
                actual = true;
            }
            // Assert
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
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ProductionService_GetProductionsByCurrentDate_Pass()
        {
            // Arrange
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater()
            {
                TheaterName = "The Language",
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

            var expected = true;
            var actual = false;
            // Act
            var readProductionsList = productionService.GetProductionsByCurrentAndFutureDate(new DateTime(2019, 3, 1), null, 1, 10); // Theater id is meant to be null

            if (readProductionsList != null)
            {
                actual = true;
            }

            // Assert
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
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProductionService_UpdateProduction_Pass()
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


            productionService.CreateProduction(production);
            dbcontext.SaveChanges();


            production.ProductionName = "The Lion King 2";
            production.StateProvince = "Utah";
            production.DirectorLastName = "Mangos";

            var expected = new List<string>()
            {
                "The Lion King 2",
                "Utah",
                "Mangos"
            };


            // Act
            var actual = productionService.UpdateProduction(production);
            dbcontext.SaveChanges();

            // Assert
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Assert.AreEqual(expected[0], actual.ProductionName);
            Assert.AreEqual(expected[1], actual.StateProvince);
            Assert.AreEqual(expected[2], actual.DirectorLastName);

        }

        [TestMethod]
        public void ProductionService_DeleteProduction_Pass()
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

            var expected = true;
            var actual = false;

            // Act
            productionService.DeleteProduction(production.ProductionID);
            var affectedRows = dbcontext.SaveChanges();
            if (affectedRows > 0)
            {
                actual = true;
            }

            // Assert
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Assert.AreEqual(expected, actual);

        }
       
        [TestMethod]
        public void ProductionService_DeleteProductionThatDoesNotExist_Pass()
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
                 
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();

            var expected = true;
            var actual = false;

            // Act
            try
            {
                productionService.DeleteProduction(production.ProductionID);
            }
            catch (ProductionNotFoundException ex)
            {
                actual = true;
            }

            // Assert
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProductionService_CreateProductionDateTime_Pass()
        {
            //Arrange
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

            var date = DateTime.Parse("3/23/2019 3:22:29 PM");
            var time = TimeSpan.Parse("10:30:00");

            var productionDateTime = new ProductionDateTime(production.ProductionID, date, time);

            var expected = true;
            var actual = false;

            // Act
            productionService.CreateProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();

            if (productionDateTime.ProductionDateTimeId > 0)
            {
                actual = true;
            }

            // Assert
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void ProductionService_UpdateProductionDateTime_Pass()
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
            productionDateTime.Time = TimeSpan.Parse("9:30:00"); // Info: Timespan format is hh:mm:ss Ex. 09:30:00

            var expected = new
            {
                DateTime = DateTime.Parse("3/27/2019 3:22:29 PM"),
                TimeSpan = TimeSpan.Parse("9:30:00")
            };

            // Act
            var actual = productionService.UpdateProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();

            // Assert
            productionService.DeleteProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Assert.AreEqual(expected.DateTime, actual.Date);
            Assert.AreEqual(expected.TimeSpan, actual.Time);
        }

        [TestMethod]
        public void ProductionService_DeleteProductionDateTime_Pass()
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

            var expected = true;
            var actual = false;

            // Act
            productionService.DeleteProductionDateTime(productionDateTime);
            var affectedRows = dbcontext.SaveChanges();
            
            if (affectedRows > 0)
            {
                actual = true;
            }

            // Assert
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Assert.AreEqual(expected, actual);
        }

        /*
        Unit Test: Testing if Mocked Posted File successfuly saves to the filesystem.
        */
        [TestMethod]
        public void ProductionService_UploadProgram_Pass()
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

            var mockedPostedPdfFile = new MockPostedFile("pdf", 5000000, "productionProgramTestFile.pdf");

            var extension = Path.GetExtension(mockedPostedPdfFile.FileName);

            var currentDirectory = ConfigurationManager.AppSettings["FileDir"];

            var dir = Path.Combine(currentDirectory, "Programs/");
            var subdir = Path.Combine(dir, $"Production{production.ProductionID}/");
            var filePath = Path.Combine(subdir, $"{production.ProductionID}{extension}");
 

            var expected = true;
            var actual = false;

            // Act
            productionService.SaveProgram(production.ProductionID, mockedPostedPdfFile);

            if (File.Exists(filePath))
            {
                actual = true;
            }

            // Assert
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            File.Delete(filePath);
            Directory.Delete(subdir);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void ProductionService_SavePhotos_Pass()
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

            var mockedPostedPdfFile = new MockPostedFile("jpg", 5000000, "productionPhotoTestFile.jpg");

            var extension = Path.GetExtension(mockedPostedPdfFile.FileName);

            var currentDirectory = ConfigurationManager.AppSettings["FileDir"];

            var dir = Path.Combine(currentDirectory, "Photos/");
            var subdir = Path.Combine(dir, $"Production{production.ProductionID}/");
            var filePath = Path.Combine(subdir, $"{production.ProductionID}-0{extension}");


            var expected = true;
            var actual = false;

            // Act
            productionService.SavePhoto(production.ProductionID, mockedPostedPdfFile);

            if (File.Exists(filePath))
            {
                actual = true;
            }
            // Assert
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            File.Delete(filePath);
            Directory.Delete(subdir);
            Assert.AreEqual(expected, actual);
        }
    }
}
