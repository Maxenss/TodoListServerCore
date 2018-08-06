using Moq;
using System;
using System.Collections.Generic;
using ToDoListServerCore.Controllers;
using ToDoListServerCore.DB;
using ToDoListServerCore.Models.DTO;
using Xunit;

namespace ToDoListServerCore.Tests
{
    public class AccountControllerTest
    {

        private List<User> GetTestUsers() {
            User user = new User(1, "Test1", "Email1", "Pass1");

            List<User> users = new List<User>();
            users.Add(user);

            return users;
        }

     //  [Fact]
     //  public void IndexViewDataMessage()
     //  {
     //      // Arrange
     //      AccountController controller = new AccountController(;
     //
     //      // Act
     //      ViewResult result = controller.Index() as ViewResult;
     //
     //      // Assert
     //      Assert.Equal("Hello world!", result?.ViewData["Message"]);
     //  }
    }
}
