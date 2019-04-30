using System;
using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLayer.Services;

namespace ServiceLayer.Test
{
    [TestClass]
    public class AuthorizationTests
    {
        [TestMethod]
        public void AuthorizationService_UserHasPermission_Pass()
        {
            // Arrange
            BroadwayBuilderContext broadwayBuilderContext = new BroadwayBuilderContext();

            var username = "abixcastro@gmail.com";
            var firstName = "Abi";
            var lastName = "Castro";
            int age = 24;
            var dob = new DateTime(1994, 1, 7);
            var streetAddress ="address1"; 
            var city = "San Diego";
            var stateProvince = "California";
            var country = "United States";
            var enable = true;

            var user = new User(username, firstName, lastName, age, dob, streetAddress,city, stateProvince, country, enable,Guid.NewGuid());
            var permission = new Permission("RateShow", true);
            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");

            
            var expected = true;
            var actual = false;
            

            var service = new AuthorizationService(broadwayBuilderContext);
            var userService = new UserService(broadwayBuilderContext);
            var theaterService = new TheaterService(broadwayBuilderContext);
            var permissionService = new PermissionService(broadwayBuilderContext);

            //Adding data into tables
            permissionService.CreatePermission(permission);
            broadwayBuilderContext.SaveChanges();
            userService.CreateUser(user);
            broadwayBuilderContext.SaveChanges();
            theaterService.CreateTheater(theater);
            broadwayBuilderContext.SaveChanges();
            userService.AddUserPermission(user, permission, theater);
            broadwayBuilderContext.SaveChanges();


            // Act 
            actual = service.HasPermission(user,permission,theater);

            UserPermission userPermission = userService.GetUserPermission(user, permission, theater);
            userService.DeleteUserPermission(userPermission);
            broadwayBuilderContext.SaveChanges();
            userService.DeleteUser(user);
            permissionService.DeletePermission(permission);
            theaterService.DeleteTheater(theater);
            broadwayBuilderContext.SaveChanges();

            // Assert
            Assert.AreEqual(expected, actual);

            
        }


        [TestMethod]
        public void AuthorizationService_UserDoesNotHavePermission_Pass()
        {
            // Arrange
            var username = "abixcastro@gmail.com";
            var firstName = "Abi";
            var lastName = "Castro";
            int age = 24;
            var dob = new DateTime(1994, 1, 7);
            var streetAddress = "address";
            var city = "San Diego";
            var stateProvince = "California";
            var country = "United States";
            var enable = true;

            var user = new User(username, firstName, lastName, age, dob, streetAddress,city, stateProvince, country, enable,Guid.NewGuid());
            var permission = new Permission("RateShow", true);
            var theater = new Theater("someTheater", "Regal", "theater st", "LA", "CA", "US", "323323");

            BroadwayBuilderContext broadwayBuilderContext = new BroadwayBuilderContext();

            var expected = false;
            var actual = true;


            var service = new AuthorizationService(broadwayBuilderContext);
            var userService = new UserService(broadwayBuilderContext);
            var permissionService = new PermissionService(broadwayBuilderContext);
            var theaterService = new TheaterService(broadwayBuilderContext);

            permissionService.CreatePermission(permission);
            broadwayBuilderContext.SaveChanges();
            theaterService.CreateTheater(theater);
            broadwayBuilderContext.SaveChanges();
            userService.CreateUser(user);
            broadwayBuilderContext.SaveChanges();

            // Act 
            actual = service.HasPermission(user, permission,theater);

            
            userService.DeleteUser(user);
            theaterService.DeleteTheater(theater);
            permissionService.DeletePermission(permission);
            broadwayBuilderContext.SaveChanges();

            // Assert
            Assert.AreEqual(expected, actual);


        }


    }
}
