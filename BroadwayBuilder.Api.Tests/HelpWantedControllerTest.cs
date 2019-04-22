using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web.Http.Results;
using BroadwayBuilder.Api.Controllers;
using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace BroadwayBuilder.Api.Tests
{
    [TestClass]
    public class HelpWantedControllerTest
    {
        [TestMethod]
        public void PostShouldAddTheaterJob()//need to update
        {
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);
            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();
            //Arrange
            var controller = new HelpWantedController();
            TheaterJobPosting job = new TheaterJobPosting(theater.TheaterID,"test", "test", "test", "test", "test","testType");
            //Act
            var actionResult = controller.CreateTheaterJob(job);
            var response = actionResult as NegotiatedContentResult<TheaterJobPosting>;
            var content = response.Content;

            var jobservice = new HelpWantedService(dbcontext);
            jobservice.DeleteTheaterJob(content);
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            ////Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
            //Assert.AreEqual("Theater Job Posting Created",content);
            Assert.AreEqual((HttpStatusCode)201,response.StatusCode);


        }

        [TestMethod]
        public void PutShouldUpdateTheaterJob() {
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);
            var theaterJobService = new HelpWantedService(dbcontext);
            //var expected = true;
            //var actual = false;

            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();
            var jobPosting = new TheaterJobPosting(theater.TheaterID, "intern", "some decription", "title", "hours", "some requirements", "testType");
            theaterJobService.CreateTheaterJob(jobPosting);
            dbcontext.SaveChanges();
            var controller = new HelpWantedController();

            //Act
            jobPosting.Description = "testing";
            var actionResult = controller.EditTheaterJob(jobPosting);
            var response = actionResult as NegotiatedContentResult<string>;
            var content = response.Content;

            theaterJobService.DeleteTheaterJob(jobPosting);
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
            ////Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual("Updated Job Posting", content);
            Assert.AreEqual((HttpStatusCode)202,response.StatusCode);
        }

        [TestMethod]
        public void DeleteShouldDeleteTheaterJob()
        {
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);
            var theaterJobService = new HelpWantedService(dbcontext);
            //var expected = true;
            //var actual = false;

            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();
            var jobPosting = new TheaterJobPosting(theater.TheaterID, "intern", "some decription", "title", "hours", "some requirements", "testType");
            theaterJobService.CreateTheaterJob(jobPosting);
            dbcontext.SaveChanges();
            var controller = new HelpWantedController();

            //Act
            var actionResult = controller.DeleteTheaterJob(jobPosting.HelpWantedID);
            var response = actionResult as NegotiatedContentResult<string>;
            var content = response.Content;

            var dbcontext2 = new BroadwayBuilderContext();
            var theaterService2 = new TheaterService(dbcontext2);
            theaterService2.DeleteTheater(theater);
            dbcontext2.SaveChanges();
            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual("Successfully Deleted Job Posting", content);
            Assert.AreEqual((HttpStatusCode)202, response.StatusCode);

        }

        [TestMethod]
        public void GetShouldGetAllTheaterJobs()
        {
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);
            var theaterJobService = new HelpWantedService(dbcontext);
            //var expected = true;
            //var actual = false;

            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();
            var jobPosting = new TheaterJobPosting(theater.TheaterID, "intern", "some decription", "title", "hours", "some requirements", "testType");
            theaterJobService.CreateTheaterJob(jobPosting);
            dbcontext.SaveChanges();
            //Arrange
            var controller = new HelpWantedController();
            var currentPage = 1;
            var numberOfItems = 100;
            //Act
            var actionResult = controller.GetTheaterJobs(theater.TheaterID, currentPage, numberOfItems);
            var response = actionResult as NegotiatedContentResult<IEnumerable>;
            var content = response.Content;
            //IEnumerable test;
            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual((HttpStatusCode)200, response.StatusCode);

            theaterJobService.DeleteTheaterJob(jobPosting);
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
        }
    }
}
