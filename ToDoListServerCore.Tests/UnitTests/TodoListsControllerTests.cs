using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToDoListServerCore.Controllers;
using ToDoListServerCore.DB;
using ToDoListServerCore.Models;
using ToDoListServerCore.Models.DTO;
using Xunit;

namespace ToDoListServerCore.Tests.UnitTests
{
   public class TodoListsControllerTests
    {
        private Mock<IRepository> model;
        private TodoListsController controller;

        [Fact]
        public void CreateList_ReturnCreatedToDoList() {

            CreateListDTO createListDTO = new CreateListDTO
            {
                Title = "Title1"
            };

            TodoList existTodoList = null;

            TodoList newTodoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            User user = new User(1, "Name1", "Email1", "Pass1");
            Extensions.Extensions.IsUnitTest = true;


            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetToDoLists()).Returns(GetTodoListsTest());
            model.Setup(repo => repo.GetTodoListByTitleAndUserId(
                createListDTO.Title, user.Id)).Returns(existTodoList);
            model.Setup(repo => repo.AddTodoList(newTodoList));
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);

            controller = new TodoListsController(model.Object);

            var result = controller.CreateList(createListDTO);

            Assert.IsType<CreatedResult>(result.Result);
           // var actionResult = Assert.IsType<CreatedResult>(result.Result);
            //  Assert.Equal(user, actionResult.Value);
        }

        [Fact]
        public void CreateList_EmptyTitleReturnBadRequest ()
        {
            CreateListDTO createListDTO = new CreateListDTO
            {
                Title = ""
            };

            TodoList existTodoList = null;

            TodoList newTodoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            User user = new User(1, "Name1", "Email1", "Pass1");
            Extensions.Extensions.IsUnitTest = true;


            model = new Mock<IRepository>();

            controller = new TodoListsController(model.Object);

            var result = controller.CreateList(createListDTO);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void CreateList_ReturnExistToDoList() {
            CreateListDTO createListDTO = new CreateListDTO
            {
                Title = "List1"
            };

            TodoList existTodoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            User user = new User(1, "Name1", "Email1", "Pass1");
            Extensions.Extensions.IsUnitTest = true;


            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetToDoLists()).Returns(GetTodoListsTest());
            model.Setup(repo => repo.GetTodoListByTitleAndUserId(
                createListDTO.Title, user.Id)).Returns(existTodoList);
          //  model.Setup(repo => repo.AddTodoList(newTodoList));
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);

            controller = new TodoListsController(model.Object);

            var result = controller.CreateList(createListDTO);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void SetListTitle_ReturnCorrect()
        {
            User user = new User(1, "Name1", "Email1", "Pass1");
            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "Title"
            };

            int listId = 1;
            string title = "UpdatedTitle";

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(
                listId, user.Id)).Returns(todoList);
            model.Setup(repo => repo.UpdateTodoList(todoList));

            controller = new TodoListsController(model.Object);

            var result = controller.SetListTitle(listId, title);

            // Assert
            var okObjectResult =  Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(title, (okObjectResult.Value as TodoList).Title);     
        }

        [Fact]
        public void SetListTitle_ReturnNotFound()
        {
            User user = new User(1, "Name1", "Email1", "Pass1");
            TodoList nullTodoList = null;

            int listId = 2;
            string title = "UpdatedTitle";

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(
                listId, user.Id)).Returns(nullTodoList);

            controller = new TodoListsController(model.Object);

            var result = controller.SetListTitle(listId, title);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void SetListTitle_NegativeIdBadRequest()
        {
            int listId = -1;
            string title = "UpdatedTitle";

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();

            controller = new TodoListsController(model.Object);

            var result = controller.SetListTitle(listId, title);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void SetListTitle_EmptyTitleReturnBadRequest()
        {
            int listId = 1;
            string title = "";

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();

            controller = new TodoListsController(model.Object);

            var result = controller.SetListTitle(listId, title);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void SetListTitle_NullTitleReturnBadRequest()
        {
            int listId = 1;
            string title = "";

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();

            controller = new TodoListsController(model.Object);

            var result = controller.SetListTitle(listId, title);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void GetListsForUser_CorrectReturnLists()
        {
            // Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListsByUserId(user.Id))
                .Returns(GetTodoListsTest());

            // Act
            controller = new TodoListsController(model.Object);

            // Assert
            var result = controller.GetListsForUser();
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(GetTodoListsTest().Count
                , (okObjectResult.Value as List<TodoList>).Count);
        }

        [Fact]
        public void GetListsForUser_ReturnUserNotFound() {
            // Arrange
            User user = null;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(1)).Returns(user);

            // Act
            controller = new TodoListsController(model.Object);

            // Assert
            var result = controller.GetListsForUser();
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void DeleteList_ReturnCorrectDeleted() {
            // Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(todoList.Id , user.Id))
                .Returns(todoList);
            model.Setup(repo => repo.RemoveTodoList(todoList));

            // Act
            controller = new TodoListsController(model.Object);

            // Assert
            var result = controller.DeleteList(todoList.Id);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void DeleteList_ReturnNotFoundList()
        {
            // Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            TodoList todoList = null;
            int todoListID = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(todoListID, user.Id))
                .Returns(todoList);

            // Act
            controller = new TodoListsController(model.Object);
            var result = controller.DeleteList(todoListID);

            // Assert            
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void DeleteList_ReturnNotFoundUser()
        {
            // Arrange
            User user = null;
            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };
            int userId = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(userId)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(todoList.Id, userId))
                .Returns(todoList);

            // Act
            controller = new TodoListsController(model.Object);
            var result = controller.DeleteList(todoList.Id);

            // Assert
            var okObjectResult = Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetTaskList_ReturnCorrectModel()
        {
            // Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(todoList.Id, user.Id))
                .Returns(todoList);

            // Act
            controller = new TodoListsController(model.Object);
            var result = controller.GetTaskList(todoList.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetTaskList_ReturnNotFoundList()
        {
            // Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            TodoList todoList = null;
            int listID = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(listID, user.Id))
                .Returns(todoList);

            // Act
            controller = new TodoListsController(model.Object);
            var result = controller.GetTaskList(listID);

            // Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetTaskList_ReturnNotFoundUser()
        {
            // Arrange
            User user = null;
            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };
            int userId = 1;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(userId)).Returns(user);

            // Act
            controller = new TodoListsController(model.Object);
            var result = controller.GetTaskList(todoList.Id);

            // Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        private List<TodoList> GetTodoListsTest() {
            TodoList todoList1 = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            List<TodoList> todoLists = new List<TodoList>();

            todoLists.Add(todoList1);

            return todoLists;
        }
    }
}