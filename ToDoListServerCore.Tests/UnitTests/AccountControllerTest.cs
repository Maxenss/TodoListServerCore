﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using ToDoListServerCore.Controllers;
using ToDoListServerCore.DB;
using ToDoListServerCore.Models.DTO;
using Xunit;

namespace ToDoListServerCore.Tests.UnitTests
{
    public class AccountControllerTest
    {
        private Mock<IRepository> model;
        private AccountController controller;

        public AccountControllerTest()
        {
            
        }

        [Fact]
        public void SignUpTest_ReturnEmailExist()
        {
            SignUpDTO signUpDTO = new SignUpDTO {
                Email = "Email1",
                Password = "Pass1",
                Name = "Name1"
            };

            User user = new User(1, "Name1", "Email1", "Pass1");


            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUsers()).Returns(GetTestUsers());
            model.Setup(repo => repo.GetUserByEmail("Email1")).Returns(user);

            controller = new AccountController(model.Object);

            var result = controller.SignUp(signUpDTO);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void SignUpTest_ReturnRegisterCorrect()
        {
            SignUpDTO signUpDTO = new SignUpDTO
            {
                Email = "test@gmail.com",
                Password = "123456",
                Name = "Name1"
            };

            User user = new User(1, "Name1", "Email1", "Pass1");
            User nullUser = null;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUsers()).Returns(GetTestUsers());
            model.Setup(repo => repo.GetUserByEmail("Email1")).Returns(nullUser);
            model.Setup(repo => repo.AddUser(user));

            controller = new AccountController(model.Object);

            var result = controller.SignUp(signUpDTO);

            //Assert.IsType<CreatedResult>(result.Result);
            var actionResult = Assert.IsType<CreatedResult>(result.Result);
          //  Assert.Equal(user, actionResult.Value);

        }

        [Fact]
        public void SignUpTest_SendEmptyFields()
        {
            SignUpDTO signUpDTO = new SignUpDTO
            {
                Email = "",
                Password = "",
                Name = ""
            };

            User user = new User(1, "Name1", "Email1", "Pass1");


            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUsers()).Returns(GetTestUsers());
            model.Setup(repo => repo.GetUserByEmail("Email1")).Returns(user);

            controller = new AccountController(model.Object);

            var result = controller.SignUp(signUpDTO);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void SignUpTest_SendNullFields()
        {
            SignUpDTO signUpDTO = new SignUpDTO
            {
                Email = null,
                Password = null,
                Name = null
            };

            User user = new User(1, "Name1", "Email1", "Pass1");


            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUsers()).Returns(GetTestUsers());
            model.Setup(repo => repo.GetUserByEmail("Email1")).Returns(user);

            controller = new AccountController(model.Object);

            var result = controller.SignUp(signUpDTO);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void SignUpTest_SendIncorrectEmail()
        {
            SignUpDTO signUpDTO = new SignUpDTO
            {
                Email = "vasvsagas",
                Password = "123456",
                Name = "fsaf"
            };

            User user = new User(1, "Name1", "Email1", "Pass1");


            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUsers()).Returns(GetTestUsers());
            model.Setup(repo => repo.GetUserByEmail("Email1")).Returns(user);

            controller = new AccountController(model.Object);

            var result = controller.SignUp(signUpDTO);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void SignUpTest_SendShortPasswordFields()
        {
            SignUpDTO signUpDTO = new SignUpDTO
            {
                Email = "test@gmail.com",
                Password = "123",
                Name = "fsaf"
            };

            User user = new User(1, "Name1", "Email1", "Pass1");


            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUsers()).Returns(GetTestUsers());
            model.Setup(repo => repo.GetUserByEmail("Email1")).Returns(user);

            controller = new AccountController(model.Object);

            var result = controller.SignUp(signUpDTO);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void SignInTest_ReturnUserNotFound() {
            SignInDTO signInDTO = new SignInDTO
            {
                Email = "test@gmail.com",
                Password = "Pass2",
            };

            User user = null;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserByEmailAndPassword(
                signInDTO.Email, signInDTO.Password)).Returns(user);

            controller = new AccountController(model.Object);

            var result = controller.SignIn(signInDTO);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void SignInTest_ReturnAuthCorrect()
        {
            SignInDTO signInDTO = new SignInDTO
            {
                Email = "test@gmail.com",
                Password = "Pass1",
            };

            User user = new User(1, "Name1", "Email1", "Pass1");

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserByEmailAndPassword(
                signInDTO.Email, signInDTO.Password)).Returns(user);

            controller = new AccountController(model.Object);

            var result = controller.SignIn(signInDTO);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void SignInTest_ReturnModelStateIsNotValid()
        {
            SignInDTO signInDTO = null;

            model = new Mock<IRepository>();

            controller = new AccountController(model.Object);

            var result = controller.SignIn(signInDTO);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        private List<User> GetTestUsers() {
            User user = new User(1, "Test1", "Email1", "Pass1");

            List<User> users = new List<User>();
            users.Add(user);

            return users;
        }
    }
}
