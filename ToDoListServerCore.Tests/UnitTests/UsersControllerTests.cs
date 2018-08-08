﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToDoListServerCore.Controllers;
using ToDoListServerCore.DB;
using Xunit;

namespace ToDoListServerCore.Tests.UnitTests
{
   public class UsersControllerTests
    {
        private Mock<IRepository> model;
        private UsersController controller;

        [Fact]
        public void DeleteUser_ReturnCorrectDeletedUser() {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.RemoveUser(user));
            #endregion

            #region Act
            controller = new UsersController(model.Object);
            var result = controller.DeleteUser();
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public void DeleteUser_ReturnUserNotFound()
        {
            #region Arrange
            User nullUser = null;
            int userId = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(userId)).Returns(nullUser);
            #endregion

            #region Act
            controller = new UsersController(model.Object);
            var result = controller.DeleteUser();
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<NotFoundResult>(result.Result);
            #endregion
        }
    }
}