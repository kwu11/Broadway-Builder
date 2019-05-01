using System;
using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace ServiceLayer.Test
{
    [TestClass]
    public class ResumeServiceTest
    {
        [TestMethod]
        public void CreateResume_Should_Pass()
        {
            //Arrange
            var context = new BroadwayBuilderContext();
            var resumeService = new ResumeService(context);
            var userService = new UserService(context);
            var expected = true;
            var actual = false;
            var user = new User("resumetest2@email.com","FN","LN",19,new DateTime(1994, 1, 7),"address","LA","CA","USA",true, Guid.NewGuid());
            userService.CreateUser(user);
            context.SaveChanges();

            var resume = new Resume(user.UserId,Guid.NewGuid());
            //Act
            resumeService.CreateResume(resume);
            var results = context.SaveChanges();
            if (results > 0)
            {
                actual = true;
            }
            resumeService.DeleteResume(resume);
            userService.DeleteUser(user);
            context.SaveChanges();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeleteResume_Should_Pass()
        {
            //Arrange
            var context = new BroadwayBuilderContext();
            var resumeService = new ResumeService(context);
            var userService = new UserService(context);
            var expected = true;
            var actual = false;
            var user = new User("resumetest2@email.com", "FN", "LN", 19, new DateTime(1994, 1, 7), "address", "LA", "CA", "USA", true, Guid.NewGuid());
            userService.CreateUser(user);
            context.SaveChanges();

            var resume = new Resume(user.UserId, Guid.NewGuid());
            resumeService.CreateResume(resume);
            context.SaveChanges();

            //Act
            resumeService.DeleteResume(resume);
            var results = context.SaveChanges();
            if (results > 0)
            {
                actual = true;
            }

            //Assert
            Assert.AreEqual(expected, actual);

            userService.DeleteUser(user);
            context.SaveChanges();
        }

        [TestMethod]
        public void GetResumeById_Should_Pass()
        {
            //Arrange
            var context = new BroadwayBuilderContext();
            var resumeService = new ResumeService(context);
            var userService = new UserService(context);
            //var expected = true;
            //var actual = false;
            var user = new User("resumetest2@email.com", "FN", "LN", 19, new DateTime(1994, 1, 7), "address", "LA", "CA", "USA", true, Guid.NewGuid());
            userService.CreateUser(user);
            context.SaveChanges();

            var resume = new Resume(user.UserId, Guid.NewGuid());
            resumeService.CreateResume(resume);
            var results = context.SaveChanges();

            //Act
            var GetResume = resumeService.GetResumeById(resume.ResumeID);

            //Assert
            Assert.AreEqual(resume.ResumeID, GetResume.ResumeID);
            Assert.AreEqual(resume.UserId, GetResume.UserId);
            resumeService.DeleteResume(resume);
            userService.DeleteUser(user);
            context.SaveChanges();
        }

        [TestMethod]
        public void UpdateResume_Should_Pass()
        {
            //Arrange
            var context = new BroadwayBuilderContext();
            var resumeService = new ResumeService(context);
            var userService = new UserService(context);
            var expected = true;
            var actual = false;
            var user = new User("resumetest2@email.com", "FN", "LN", 19, new DateTime(1994, 1, 7), "address", "LA", "CA", "USA", true, Guid.NewGuid());
            userService.CreateUser(user);
            context.SaveChanges();

            var resume = new Resume(user.UserId, Guid.NewGuid());
            resumeService.CreateResume(resume);
            context.SaveChanges();

            //Act
            resume.ResumeGuid = Guid.NewGuid();
            resumeService.UpdateResume(resume);
            var results = context.SaveChanges();
            if (results > 0)
            {
                actual = true;
            }

            var UpdatedResume = resumeService.GetResumeById(resume.ResumeID);

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(resume.ResumeGuid, UpdatedResume.ResumeGuid);

            resumeService.DeleteResume(resume);
            userService.DeleteUser(user);
            context.SaveChanges();
        }
    }
}
