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
        public void RoleService_CreateRole_Pass()
        {
            // Arrange
            var NewRole = new Role("GENERAL",true);

            bool expected = true;
            bool actual = false;

            var context = new BroadwayBuilderContext();
            var roleService = new RoleService(context);

            // Act
            roleService.CreateRole(NewRole);
            var numOfAffectedRows = context.SaveChanges();

            if (numOfAffectedRows > 0)
            {
                actual = true;
            }


            // Assert
            Assert.AreEqual(expected, actual);
            roleService.DeleteRole(NewRole);
            context.SaveChanges();

        }

        [TestMethod]
        public void RoleService_GetRole_Pass()
        {
            // Arrange

            string role = "ADMIN";

            var NewRole = new Role(role,true);

            var expected = NewRole;

            var context = new BroadwayBuilderContext();
            var roleService = new RoleService(context);
            roleService.CreateRole(NewRole);
            context.SaveChanges();

            // Act
            var actual = roleService.GetRole(NewRole.RoleID);

            roleService.DeleteRole(NewRole);
            context.SaveChanges();


            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void RoleService_DeleteRole_Pass()
        {
            // Arrange
            var NewRole = new Role("GENERAL",true);

            bool expected = true;
            bool actual = false;

            var context = new BroadwayBuilderContext();
            var roleService = new RoleService(context);
            roleService.CreateRole(NewRole);
            context.SaveChanges();

            // Act
            roleService.DeleteRole(NewRole);
            var numOfAffectedRows = context.SaveChanges();

            if (numOfAffectedRows > 0)
            {
                actual = true;
            }

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void RoleService_ShouldNotDeleteRoleAgain()
        {
            // Arrange
            var NewRole = new Role("GENERAL", true);

            bool expected = true;
            bool actual = false;

            var context = new BroadwayBuilderContext();
            var roleService = new RoleService(context);
            roleService.CreateRole(NewRole);
            context.SaveChanges();

            roleService.DeleteRole(NewRole);
            context.SaveChanges();

            //Act
            roleService.DeleteRole(NewRole);
            var result = context.SaveChanges();
            if (result == 0)
            {
                actual = true;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RoleService_ShouldReturnNullForNonExistingRole()
        {
            //Arrange
            var context = new BroadwayBuilderContext();
            var roleService = new RoleService(context);
            int nonExisitingRoleId = -1000;
            
            //Act 
            var actual = roleService.GetRole(nonExisitingRoleId);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void RoleService_ShouldUpdateRole()
        {
            // Arrange
            var NewRole = new Role("GENERAL", true);

            bool expected = true;
            bool actual = false;

            var context = new BroadwayBuilderContext();
            var roleService = new RoleService(context);
            roleService.CreateRole(NewRole);
            context.SaveChanges();

            //Act
            NewRole.RoleName = "ADMIN";
            roleService.UpdateRole(NewRole);
            var result = context.SaveChanges();
            if (result > 0)
            {
                actual = true;
            }

            Role updatedRole = roleService.GetRole(NewRole.RoleID);

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(NewRole.RoleName, updatedRole.RoleName);

            roleService.DeleteRole(NewRole.RoleID);
            context.SaveChanges();
        }
    }
}


