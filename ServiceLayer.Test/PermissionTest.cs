using System;
using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceLayer.Test
{
    [TestClass]
    public class PermissionTest
    {
        [TestMethod]
        public void Permission_CreatePermission_Pass()
        {
            //Arrange
            var aString = "Rate Show";
            var permission = new Permission(aString);
            var context = new BroadwayBuilderContext();
            var permissionService = new PermissionService(context);

            var expected = true;
            var actual = false;

            //Act
            permissionService.CreatePermission(permission);
            var save = context.SaveChanges();
            if (save > 0)
            {
                actual = true; 
            }

            permissionService.DeletePermission(permission);
            context.SaveChanges();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Permission_GetPermission_Pass()
        {
            //Arrange
            var aString = "RateShow";
            var permission = new Permission(aString);
            var context = new BroadwayBuilderContext();
            var permissionService = new PermissionService(context);
            var expected = true;
            var actual = false;

            //Act
            permissionService.CreatePermission(permission);
            context.SaveChanges();
            Permission getPermission = permissionService.GetPermission(aString);
            if (getPermission != null)
                actual = true;

            permissionService.DeletePermission(permission);
            context.SaveChanges();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Permission_DeletePermission_Pass()
        {
            //Arrange
            var aString = "Rate Show";
            var permission = new Permission(aString);
            var context = new BroadwayBuilderContext();
            var permissionService = new PermissionService(context);

            var expected = true;
            var actual = false;

            //Act
            permissionService.CreatePermission(permission);
            context.SaveChanges();
            permissionService.DeletePermission(permission);
            var save = context.SaveChanges();
            if (save > 0)
            {
                actual = true;
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
