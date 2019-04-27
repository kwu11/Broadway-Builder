using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using ServiceLayer.Services;
using System.Collections.Generic;
using ServiceLayer.Exceptions;

namespace ServiceLayer.Test
{
    [TestClass]
    public class ProductionServiceTest
    {
        [TestMethod]
        public void ProductionService_CreateProduction_Pass()
        {
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            // Arrange
            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionName = "The Lion King";
            var directorFirstName = "Jane";
            var directorLastName = "Doe";
            var street = "Anahiem";
            var city = "Long Beach";
            var stateProvince = "California";
            var country = "United States";
            var zipcode = "919293";

            
            var production = new Production(theater.TheaterID, productionName, directorFirstName, directorLastName, street, city, stateProvince, country, zipcode);

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

            var theater = new Theater("The Language", "Pantene", "123 Sesame St", "San Diego", "California", "U.S", "8587175730");
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

            // Act

            // Assert

        }


        [TestMethod]
        public void ProductionService_GetProductionsByCurrentDate_Pass()
        {
            // Arrange

            // Act

            // Assert

        }

        [TestMethod]
        public void ProductionService_UpdateProduction_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater("The Magicians", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionService = new ProductionService(dbcontext);

            var productionName = "The Lion King";
            var directorFirstName = "Joan";
            var directorLastName = "Doe";
            var street = "123 Anahiem St";
            var city = "Long Beach";
            var stateProvince = "California";
            var country = "United States";
            var zipcode = "919293";

            var production = new Production(theater.TheaterID, productionName, directorFirstName, directorLastName, street, city, stateProvince, country, zipcode);
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

            var theater = new Theater("The Lions", "Opi", "123 Sesame St", "San Diego", "California", "U.S", "8587175730");

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
        /*
        TODO: Usure if it is good practice to use try and catch in a test and to throw an exception...
        When i removed that, the method would throw the exception and the test would fail. I want to test that an exception is actually thrown
        so maybe the fail is what I would like to see....Expected Exception seems to work but.. Please do more research
        */
        [TestMethod]
        [ExpectedException(typeof(ProductionNotFoundException), "A production with the id was not found")]
        public void ProductionService_DeleteProductionAgain_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater("The Lions", "Opi", "123 Sesame St", "San Diego", "California", "U.S", "8587175730");

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

            var expected = false;
            var actual = true;

            //try
            //{

            //throw new ProductionNotFoundException($"Production does not exist! with id: {production.ProductionID}"); ;
            //}
            //catch (Exception e)
            //{
            //    var expected = e;
            //}

            // Act
            //try
            //{

            productionService.DeleteProduction(production.ProductionID);
            var affectedRows = dbcontext.SaveChanges();
            if (affectedRows == 0)
            {
                actual = false;
            }
            //}
            //catch (Exception e)
            //{
            //    var actual = e;
            //}

            // Assert
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            //Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void ProductionService_CreateProductionDateTime_Pass()
        {
            //Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater("The Magicians", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionService = new ProductionService(dbcontext);

            var productionName = "The Lion King";
            var directorFirstName = "Joan";
            var directorLastName = "Doe";
            var street = "123 Anahiem St";
            var city = "Long Beach";
            var stateProvince = "California";
            var country = "United States";
            var zipcode = "919293";

            var production = new Production(theater.TheaterID, productionName, directorFirstName, directorLastName, street, city, stateProvince, country, zipcode);
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

            var theater = new Theater("The Magicians 3 ", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionService = new ProductionService(dbcontext);

            var productionName = "The Lion King 6";
            var directorFirstName = "Joan";
            var directorLastName = "Doe";
            var street = "123 Anahiem St";
            var city = "Long Beach";
            var stateProvince = "California";
            var country = "United States";
            var zipcode = "919293";

            var production = new Production(theater.TheaterID, productionName, directorFirstName, directorLastName, street, city, stateProvince, country, zipcode);
            productionService.CreateProduction(production);
            dbcontext.SaveChanges();

            var date = DateTime.Parse("3/28/2019 3:22:29 PM");
            var time = TimeSpan.Parse("11:30:00");

            var productionDateTime = new ProductionDateTime(production.ProductionID, date, time);
            productionService.CreateProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();

            productionDateTime.Date = DateTime.Parse("3/27/2019 3:22:29 PM");
            productionDateTime.Time = TimeSpan.Parse("9:30:00"); // Info: Timespan format is hh:mm:ss Ex. 09:30:00

            var expected = new List<string>()
            {
                "3/27/2019 3:22:29 PM",
                "09:30:00"
            };

            // Act
            var actual = productionService.UpdateProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();

            var dateString = actual.Date.ToString();
            var timeString = actual.Time.ToString(@"hh\:mm\:ss");
            

            // Assert
            productionService.DeleteProductionDateTime(productionDateTime);
            dbcontext.SaveChanges();
            productionService.DeleteProduction(production.ProductionID);
            dbcontext.SaveChanges();
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            Assert.AreEqual(expected[0], dateString);
            Assert.AreEqual(expected[1], timeString);
        }

        [TestMethod]
        public void ProductionService_DeleteProductionDateTime_Pass()
        {
            // Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);

            var theater = new Theater("The Magicians 4 ", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();

            var productionService = new ProductionService(dbcontext);

            var productionName = "The Lion King 7";
            var directorFirstName = "Joan";
            var directorLastName = "Doe";
            var street = "123 Anahiem St";
            var city = "Long Beach";
            var stateProvince = "California";
            var country = "United States";
            var zipcode = "919293";

            var production = new Production(theater.TheaterID, productionName, directorFirstName, directorLastName, street, city, stateProvince, country, zipcode);
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
        Not sure if these get tested here.. But moste likely since it is an integration test with the controller.
        */
        [TestMethod]
        public void ProductionService_UploadProgram_Pass()
        {
            // Arrange

            // Act

            // Assert

        }

        [TestMethod]
        public void ProductionService_UploadPhotos_Pass()
        {
            // Arrange

            // Act

            // Assert

        }
    }
}
