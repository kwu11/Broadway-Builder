using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using ServiceLayer.Services;

namespace ServiceLayer.Test
{
    [TestClass]
    public class RoleServiceTest
    {

        [TestMethod]
        public void RoleService_GetRole_Pass()
        {
            // Arrange  
            var expected = true;

            var context = new BroadwayBuilderContext();
            var roleService = new RoleService(context);
         

            // Act
            var role = roleService.GetRole(DataAccessLayer.Enums.RoleEnum.TheaterAdmin);

            var actual = false;

            if (role != null)
            {
                actual = true;
            }

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);

        }
    }
}


