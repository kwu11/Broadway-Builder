using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using ServiceLayer.Services;
using System.Collections.Generic;

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
            
            /* Info: Had to cast to int because in production entity model int was made into a Nullable<int> or int? for data validation purposes
               If we make model in frontend then we can remove this cast to int and it will make things cleaner
           */
            var productionDateTime = new ProductionDateTime((int)production.ProductionID, date, time);

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
    }
}
