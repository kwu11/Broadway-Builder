using System;

using DataAccessLayer;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace ServiceLayer.Test

{

    [TestClass]

    public class PermissionServiceTest

    {

        [TestMethod]

        public void PermissionService_CreatePermission_Pass()

        {

            //Arrange

            var aString = "Rate Show";

            var permission = new Permission(aString,true);



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

        public void PermissionService_GetPermission_Pass()

        {

            //Arrange

            var aString = "RateShow";

            var permission = new Permission(aString,true);

            var context = new BroadwayBuilderContext();

            var permissionService = new PermissionService(context);



            var expected = true;

            var actual = false;


            //Act
            Permission getPermission = permissionService.GetPermission(DataAccessLayer.Enums.PermissionsEnum.ActivateAbusiveAccount);
            if (getPermission != null)

                actual = true;


            permissionService.DeletePermission(permission);

            context.SaveChanges();


            //Assert

            Assert.AreEqual(expected, actual);

        }


        [TestMethod]

        public void PermissionService_DeletePermission_Pass()

        {

            //Arrange

            var aString = "Rate Show";

            var permission = new Permission(aString,true);



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