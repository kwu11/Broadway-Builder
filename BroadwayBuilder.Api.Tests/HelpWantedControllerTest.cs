using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web.Http.Results;
using BroadwayBuilder.Api.Controllers;
using BroadwayBuilder.Api.Models;
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
            //Arrange
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);
            var theater = new Theater("someTheater2", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();
            var controller = new HelpWantedController();
            TheaterJobPosting job = new TheaterJobPosting(theater.TheaterID,"test", "test", "test", "test", "test","testType");
            
            //Act
            var actionResult = controller.CreateTheaterJob(job);
            var response = actionResult as NegotiatedContentResult<TheaterJobResponseModel>;
            var content = response.Content;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual((HttpStatusCode)201,response.StatusCode);

            var jobservice = new TheaterJobPostingService(dbcontext);
            jobservice.DeleteTheaterJob(content.HelpWantedID);
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
        }

        [TestMethod]
        public void PutShouldUpdateTheaterJob() {
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);
            var theaterJobService = new TheaterJobPostingService(dbcontext);

            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            dbcontext.SaveChanges();
            var jobPosting = new TheaterJobPosting(theater.TheaterID, "intern", "some decription", "title", "hours", "some requirements", "testType");
            theaterJobService.CreateTheaterJob(jobPosting);
            dbcontext.SaveChanges();
            var controller = new HelpWantedController();

            //Act
            jobPosting.Description = "testing";
            //TheaterJobResponseModel theaterJobResponseModel = new TheaterJobResponseModel(jobPosting);
            var actionResult = controller.EditTheaterJob(jobPosting);
            var response = actionResult as NegotiatedContentResult<string>;
            var content = response.Content;
            
            ////Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual("Updated Job Posting", content);
            Assert.AreEqual((HttpStatusCode)200,response.StatusCode);

            theaterJobService.DeleteTheaterJob(jobPosting.HelpWantedID);
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
        }

        [TestMethod]
        public void DeleteShouldDeleteTheaterJob()
        {
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);
            var theaterJobService = new TheaterJobPostingService(dbcontext);

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
            Assert.AreEqual((HttpStatusCode)200, response.StatusCode);

        }

        [TestMethod]
        public void GetShouldGetAllTheaterJobs()
        {
            var dbcontext = new BroadwayBuilderContext();
            var theaterService = new TheaterService(dbcontext);
            var theaterJobService = new TheaterJobPostingService(dbcontext);

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
            var response = actionResult as NegotiatedContentResult<TheaterJobResponseList>;
            //var content = response.Content;
            
            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual((HttpStatusCode)200, response.StatusCode);

            theaterJobService.DeleteTheaterJob(jobPosting.HelpWantedID);
            theaterService.DeleteTheater(theater);
            dbcontext.SaveChanges();
        }

        [TestMethod]
        public void DeleteTheaterJob_Should_ReturnErrorMessage()
        {
            //Arrange
            var controller = new HelpWantedController();
            int nonExistantID = -(int.MaxValue);

            //Act
            var actionResult = controller.DeleteTheaterJob(nonExistantID);
            var response = actionResult as NegotiatedContentResult<string>;
            var content = response.Content;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual((HttpStatusCode)404, response.StatusCode);
        }
    }
}
