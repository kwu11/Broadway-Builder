using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using ServiceLayer.Services;

namespace ServiceLayer.Test
{
    /// <summary>
    /// Summary description for TheaterServiceTest
    /// </summary>
    [TestClass]
    public class TheaterServiceTest
    {
        public TheaterServiceTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TheaterService_CreateTheater_Pass()
        {
            // Arrange
            var context = new BroadwayBuilderContext();
            var theater = new Theater("Performing Arts", "Long Beach Shakespeare Company", "40855 Theater Lane", "Long Beach", "CA", "USA", "555-955-8955");
            var theaterService = new TheaterService(context);
            var expected = true;
            var actual = false;

            // Act
            theaterService.CreateTheater(theater);
            if (context.SaveChanges() > 0) actual = true;

            // Assert
            theaterService.DeleteTheater(theater);
            context.SaveChanges();
            //Console.WriteLine("aaaaaaaaaaaaaaaa");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TheaterService_DeleteTheater_Pass()
        {
            // Arrange
            var context = new BroadwayBuilderContext();
            var theater = new Theater("Performing Arts", "Long Beach Shakespeare Company", "40855 Theater Lane", "Long Beach", "CA", "USA", "555-955-8955");
            var theaterService = new TheaterService(context);
            var expected = true;
            var actual = false;
            theaterService.CreateTheater(theater);
            context.SaveChanges();

            // Act
            theaterService.DeleteTheater(theater);
            if (context.SaveChanges() > 0) actual = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TheaterService_DeleteNonExistantTheater_Pass()
        {
            // Arrange
            var context = new BroadwayBuilderContext();
            var theater = new Theater("Performing Arts", "Long Beach Shakespeare Company", "40855 Theater Lane", "Long Beach", "CA", "USA", "555-955-8955");
            var theaterService = new TheaterService(context);
            var expected = true;
            var actual = false;

            // Act
            try
            {
                theaterService.DeleteTheater(theater);
                var changesMade = context.SaveChanges();
                if (changesMade > 0) actual = true;
            }
            catch (Exception)
            {
                actual = false;
            }

            // Assert
            finally
            {
                Assert.AreNotEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TheaterService_UpdateTheater_Pass()
        {
            // Arrange
            var context = new BroadwayBuilderContext();
            var theater = new Theater("Performing Arts", "Long Beach Shakespeare Company", "40855 Theater Lane", "Long Beach", "CA", "USA", "555-955-8955");
            var theaterService = new TheaterService(context);
            theaterService.CreateTheater(theater);
            context.SaveChanges();

            // Act
            theater.TheaterName = "Natural History";
            theater.CompanyName = "Different Company";
            var expected = theater;
            //var actual = theater;
            var actual = theaterService.UpdateTheater(theater);
            //theater.StreetAddress = "125 Bogus Lane";
            context.SaveChanges();

            theaterService.DeleteTheater(theater);
            context.SaveChanges();

            // Assert
            Assert.AreEqual(expected.TheaterName, actual.TheaterName);
            Assert.AreEqual(expected.CompanyName, actual.CompanyName);
            Assert.AreEqual(expected.StreetAddress, actual.StreetAddress);
            Assert.AreEqual(expected.City, actual.City);
            Assert.AreEqual(expected.State, actual.State);
            Assert.AreEqual(expected.Country, actual.Country);
            Assert.AreEqual(expected.PhoneNumber, actual.PhoneNumber);
        }

        [TestMethod]
        public void TheaterService_UpdateNonExistantTheater_Pass()
        {
            // Arrange
            var context = new BroadwayBuilderContext();
            var theater = new Theater("Performing Arts", "Long Beach Shakespeare Company", "40855 Theater Lane", "Long Beach", "CA", "USA", "555-955-8955");
            var theaterService = new TheaterService(context);

            // Act
            theater.TheaterName = "Natural History";
            theater.CompanyName = "Different Company";
            var expected = theater;
            var actual = theaterService.UpdateTheater(theater);
            context.SaveChanges();

            // Assert
            Assert.AreNotEqual(expected, actual);
        }
    }
}
