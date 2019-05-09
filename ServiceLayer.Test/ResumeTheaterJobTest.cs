using System;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace ServiceLayer.Test
{
    [TestClass]
    public class ResumeTheaterJobTest
    {
        [TestMethod]
        public void CreateResumeTheaterJob_Should_Create()
        {
            //Arrange
            var context = new BroadwayBuilderContext();
            var resumeService = new ResumeService(context);
            var userService = new UserService(context);
            var theaterService = new TheaterService(context);
            var theaterJobService = new TheaterJobPostingService(context);
            var resumeTheaterJobService = new ResumeTheaterJobService(context);

            var expected = true;
            var actual = false;

            var user = new User("resumetest@email.com", "FN", "LN", 19, new DateTime(1994, 1, 7), "address", "LA", "CA", "USA", true, Guid.NewGuid());
            userService.CreateUser(user);
            context.SaveChanges();

            var resume = new Resume(user.UserId, Guid.NewGuid());
            resumeService.CreateResume(resume);
            context.SaveChanges();

            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            context.SaveChanges();

            var jobPosting = new TheaterJobPosting(theater.TheaterID, "intern", "some decription", "title", "hours", "some requirements", "testType");
            theaterJobService.CreateTheaterJob(jobPosting);
            context.SaveChanges();

            //Act
            var resumeTheaterJob = new ResumeTheaterJob(jobPosting.HelpWantedID, resume.ResumeID);
            resumeTheaterJobService.CreateResumeTheaterJob(resumeTheaterJob);
            var result = context.SaveChanges();
            if (result > 0)
            {
                actual = true;
            }
            resumeTheaterJobService.DeleteResumeTheaterJob(resumeTheaterJob);
            resumeService.DeleteResume(resume);
            theaterJobService.DeleteTheaterJob(jobPosting.HelpWantedID);
            userService.DeleteUser(user);
            theaterService.DeleteTheater(theater);
            context.SaveChanges();

            //Assert 
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateResumeTheaterJob_Should_NotCreateSameResumeTheaterJob()
        {
            //Arrange
            var context = new BroadwayBuilderContext();
            var resumeService = new ResumeService(context);
            var userService = new UserService(context);
            var theaterService = new TheaterService(context);
            var theaterJobService = new TheaterJobPostingService(context);
            var resumeTheaterJobService = new ResumeTheaterJobService(context);

            var expected = true;
            var actual = false;

            var user = new User("resumetest@email.com", "FN", "LN", 19, new DateTime(1994, 1, 7), "address", "LA", "CA", "USA", true, Guid.NewGuid());
            userService.CreateUser(user);
            context.SaveChanges();

            var resume = new Resume(user.UserId, Guid.NewGuid());
            resumeService.CreateResume(resume);
            context.SaveChanges();

            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            context.SaveChanges();

            var jobPosting = new TheaterJobPosting(theater.TheaterID, "intern", "some decription", "title", "hours", "some requirements", "testType");
            theaterJobService.CreateTheaterJob(jobPosting);
            context.SaveChanges();

            //Act
            var resumeTheaterJob = new ResumeTheaterJob(jobPosting.HelpWantedID, resume.ResumeID);
            resumeTheaterJobService.CreateResumeTheaterJob(resumeTheaterJob);
            context.SaveChanges();

            try
            {
                resumeTheaterJobService.CreateResumeTheaterJob(resumeTheaterJob);
            }
            catch
            {
                actual = true;
            }
            resumeTheaterJobService.DeleteResumeTheaterJob(resumeTheaterJob);
            resumeService.DeleteResume(resume);
            theaterJobService.DeleteTheaterJob(jobPosting.HelpWantedID);
            userService.DeleteUser(user);
            theaterService.DeleteTheater(theater);
            context.SaveChanges();

            //Assert 
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeleteResumeTheaterJob_Should_Delete()
        {
            //Arrange
            var context = new BroadwayBuilderContext();
            var resumeService = new ResumeService(context);
            var userService = new UserService(context);
            var theaterService = new TheaterService(context);
            var theaterJobService = new TheaterJobPostingService(context);
            var resumeTheaterJobService = new ResumeTheaterJobService(context);

            var expected = true;
            var actual = false;

            var user = new User("resumetest@email.com", "FN", "LN", 19, new DateTime(1994, 1, 7), "address", "LA", "CA", "USA", true, Guid.NewGuid());
            userService.CreateUser(user);
            context.SaveChanges();

            var resume = new Resume(user.UserId, Guid.NewGuid());
            resumeService.CreateResume(resume);
            context.SaveChanges();

            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");
            theaterService.CreateTheater(theater);
            context.SaveChanges();

            var jobPosting = new TheaterJobPosting(theater.TheaterID, "intern", "some decription", "title", "hours", "some requirements", "testType");
            theaterJobService.CreateTheaterJob(jobPosting);
            context.SaveChanges();

            var resumeTheaterJob = new ResumeTheaterJob(jobPosting.HelpWantedID, resume.ResumeID);
            resumeTheaterJobService.CreateResumeTheaterJob(resumeTheaterJob);
            context.SaveChanges();
            //Act
            resumeTheaterJobService.DeleteResumeTheaterJob(resumeTheaterJob);
            var result = context.SaveChanges();
            if (result > 0)
            {
                actual = true;
            }
            resumeService.DeleteResume(resume);
            theaterJobService.DeleteTheaterJob(jobPosting.HelpWantedID);
            userService.DeleteUser(user);
            theaterService.DeleteTheater(theater);
            context.SaveChanges();

            //Assert 
            Assert.AreEqual(expected, actual);
        }
    }
}
