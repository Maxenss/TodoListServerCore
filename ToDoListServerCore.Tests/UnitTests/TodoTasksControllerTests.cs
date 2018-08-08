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
    public class TodoTasksControllerTests
    {
        private Mock<IRepository> model;
        private TodoTasksController controller;

        [Fact]
        public void DeleteTask_ReturnCorrectDeleted()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            TodoTask todoTask = new TodoTask
            {
                Id = 1,
                Description = "Description1",
                Title = "List1",
                ToDoListId = 1,
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoTaskById(todoTask.Id)).Returns(todoTask);
            model.Setup(repo => repo.RemoveTodoTask(todoTask));
            #endregion

            // Act
            controller = new TodoTasksController(model.Object);

            // Assert
            var result = controller.DeleteTask(todoTask.Id);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void DeleteList_ReturnNotFoundList()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            TodoTask todoTask = null;
            int todoTaskId = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(todoTaskId)).Returns(user);
            #endregion

            // Act
            controller = new TodoTasksController(model.Object);

            // Assert
            var result = controller.DeleteTask(todoTaskId);
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void CreateTask_ReturnCreatedTask()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            CreateToDoTaskDTO createToDoTaskDTO = new CreateToDoTaskDTO
            {
                ToDoListId = 1,
                Title = "Title1",
                Description = "Description1"
            };
            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };
            TodoTask createdTodoTask = new TodoTask(createToDoTaskDTO);

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(
                createToDoTaskDTO.ToDoListId, user.Id)).Returns(todoList);
            model.Setup(repo => repo.AddTodoTask(createdTodoTask));
            #endregion

            // Act
            controller = new TodoTasksController(model.Object);
            var result = controller.CreateTask(createToDoTaskDTO);

            // Assert
            var okObjectResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal(createdTodoTask.Id, (okObjectResult.Value as TodoTaskDTO).Id);
        }

        [Fact]
        public void CreateTask_ReturnUserNotFound()
        {
            #region Arrange
            User user = null;
            CreateToDoTaskDTO createToDoTaskDTO = new CreateToDoTaskDTO
            {
                ToDoListId = 1,
                Title = "Title1",
                Description = "Description1"
            };
            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };
            TodoTask createdTodoTask = new TodoTask(createToDoTaskDTO);
            int userId = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(userId)).Returns(user);
            #endregion

            // Act
            controller = new TodoTasksController(model.Object);
            var result = controller.CreateTask(createToDoTaskDTO);

            // Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void CreateTask_ReturnTodoListNotFound()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            CreateToDoTaskDTO createToDoTaskDTO = new CreateToDoTaskDTO
            {
                ToDoListId = 1,
                Title = "Title1",
                Description = "Description1"
            };
            TodoList todoList = null;
            TodoTask createdTodoTask = new TodoTask(createToDoTaskDTO);

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(
                createToDoTaskDTO.ToDoListId, user.Id)).Returns(todoList);
            #endregion

            // Act
            controller = new TodoTasksController(model.Object);
            var result = controller.CreateTask(createToDoTaskDTO);

            // Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void CreateTask_ReturnBadRequestEmptyField()
        {
            // Arrange 
            User user = new User(1, "Name1", "Email1", "Pass1");
            CreateToDoTaskDTO createToDoTaskDTO = new CreateToDoTaskDTO
            {
                ToDoListId = 1,
                Title = "",
                Description = ""
            };
            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };
            TodoTask createdTodoTask = new TodoTask(createToDoTaskDTO);

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(
                createToDoTaskDTO.ToDoListId, user.Id)).Returns(todoList);
            model.Setup(repo => repo.AddTodoTask(createdTodoTask));

            // Act
            controller = new TodoTasksController(model.Object);
            var result = controller.CreateTask(createToDoTaskDTO);

            // Assert
            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void CreateTask_ReturnBadRequestInvalidModel()
        {
            // Arrange 
            User user = new User(1, "Name1", "Email1", "Pass1");
            CreateToDoTaskDTO createToDoTaskDTO = null;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();

            // Act
            controller = new TodoTasksController(model.Object);
            var result = controller.CreateTask(createToDoTaskDTO);

            // Assert
            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void SetTaskStatus_ReturnCorrect()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");

            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            TodoTask todoTask = new TodoTask
            {
                Id = 1,
                Description = "Description1",
                Title = "List1",
                ToDoListId = 1,
                TaskStatus = TodoTask.Status.AWAIT
            };

            TodoTask.Status todoTaskStatus = TodoTask.Status.DONE;
            user.TodoLists.Add(todoList);

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoTaskById(todoList.Id))
                .Returns(todoTask);
            model.Setup(repo => repo.UpdateTodoTask(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.SetTaskStatus(todoTask.Id, todoTaskStatus);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(todoTask.TaskStatus, (okObjectResult.Value as TodoTaskDTO).TaskStatus);
            #endregion
        }

        [Fact]
        public void SetTaskStatus_ReturnTodoTaskNotFound()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");

            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            TodoTask todoTask = null;

            TodoTask.Status todoTaskStatus = TodoTask.Status.DONE;
            user.TodoLists.Add(todoList);

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoTaskById(2))
                .Returns(todoTask);
            model.Setup(repo => repo.UpdateTodoTask(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.SetTaskStatus(2, todoTaskStatus);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public void SetTaskStatus_ReturnUserNotFound()
        {
            #region Arrange
            User user = null;
            int userId = 2;

            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            TodoTask todoTask = new TodoTask
            {
                Id = 1,
                Description = "Description1",
                Title = "List1",
                ToDoListId = 1,
                TaskStatus = TodoTask.Status.AWAIT
            };

            TodoTask.Status todoTaskStatus = TodoTask.Status.DONE;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(userId)).Returns(user);
            model.Setup(repo => repo.GetTodoTaskById(todoList.Id))
                .Returns(todoTask);
            model.Setup(repo => repo.UpdateTodoTask(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.SetTaskStatus(todoTask.Id, todoTaskStatus);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public void SetTaskStatus_ReturnBadRequestModelIsInvalid()
        {
            #region Arrange
            int taskId = -1;
            TodoTask.Status status = TodoTask.Status.CANCELED;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.SetTaskStatus(taskId, status);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public void UpdateTask_ReturnCorrectUpdated()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");

            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };
            user.TodoLists.Add(todoList);

            TodoTask todoTask = new TodoTask
            {
                Id = 1,
                Description = "Description1",
                Title = "List1",
                ToDoListId = 1,
                TaskStatus = TodoTask.Status.AWAIT
            };

            UpdateToDoTaskDTO updateToDoTaskDTO = new UpdateToDoTaskDTO
            {
                TaskId = todoTask.Id,
                Description = "New Description",
                Title = "New title"
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoTaskById(updateToDoTaskDTO.TaskId))
                .Returns(todoTask);
            model.Setup(repo => repo.UpdateTodoTask(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.UpdateTask(updateToDoTaskDTO);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            TodoTask updatedTask = okObjectResult.Value as TodoTask;
            Assert.Equal(updateToDoTaskDTO.TaskId, updatedTask.Id);
            Assert.Equal(updateToDoTaskDTO.Title, updatedTask.Title);
            Assert.Equal(updateToDoTaskDTO.Description, updatedTask.Description);
            Assert.Equal(updateToDoTaskDTO.ToDoListId, updateToDoTaskDTO.ToDoListId);
            #endregion
        }

        [Fact]
        public void UpdateTask_ReturnTodoTaskNotFound()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");

            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };
            user.TodoLists.Add(todoList);

            UpdateToDoTaskDTO updateToDoTaskDTO = new UpdateToDoTaskDTO
            {
                TaskId = 1,
                Description = "New Description",
                Title = "New title"
            };

            TodoTask todoTask = null;
            int todoTaskId = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoTaskById(todoTaskId))
                .Returns(todoTask);
            model.Setup(repo => repo.UpdateTodoTask(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.UpdateTask(updateToDoTaskDTO);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public void UpdateTask_ReturnUserNotFound()
        {
            #region Arrange
            User user = null;
            int userId = 2;

            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };

            TodoTask todoTask = new TodoTask
            {
                Id = 1,
                Description = "Description1",
                Title = "List1",
                ToDoListId = 1,
                TaskStatus = TodoTask.Status.AWAIT
            };

            UpdateToDoTaskDTO updateToDoTaskDTO = new UpdateToDoTaskDTO
            {
                TaskId = todoTask.Id,
                Description = "New Description",
                Title = "New title"
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(userId)).Returns(user);
            model.Setup(repo => repo.GetTodoTaskById(updateToDoTaskDTO.TaskId))
                .Returns(todoTask);
            model.Setup(repo => repo.UpdateTodoTask(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.UpdateTask(updateToDoTaskDTO);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public void UpdateTask_ReturnBadRequestNotValidModelState()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");

            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };
            user.TodoLists.Add(todoList);

            TodoTask todoTask = new TodoTask
            {
                Id = 1,
                Description = "Description1",
                Title = "List1",
                ToDoListId = 1,
                TaskStatus = TodoTask.Status.AWAIT
            };

            UpdateToDoTaskDTO updateToDoTaskDTO = null;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoTaskById(todoTask.Id))
                .Returns(todoTask);
            model.Setup(repo => repo.UpdateTodoTask(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.UpdateTask(updateToDoTaskDTO);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public void UpdateTask_ReturnBadRequestEmptyFields()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");

            TodoList todoList = new TodoList
            {
                Id = 1,
                UserId = 1,
                Title = "List1"
            };
            user.TodoLists.Add(todoList);

            TodoTask todoTask = new TodoTask
            {
                Id = 1,
                Description = "Description1",
                Title = "List1",
                ToDoListId = 1,
                TaskStatus = TodoTask.Status.AWAIT
            };

            UpdateToDoTaskDTO updateToDoTaskDTO = new UpdateToDoTaskDTO
            {
                TaskId = 0,
                Description = "",
                Title = ""
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(user);
            model.Setup(repo => repo.GetTodoTaskById(updateToDoTaskDTO.TaskId))
                .Returns(todoTask);
            model.Setup(repo => repo.UpdateTodoTask(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.UpdateTask(updateToDoTaskDTO);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            #endregion
        }
    }
}