using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void DeleteTaskTest_ReturnCorrectDeleted()
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
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoTaskById(todoTask.Id))
                .Returns(Task.FromResult(todoTask));
            model.Setup(repo => repo.RemoveTodoTask(todoTask));
            #endregion

            // Act
            controller = new TodoTasksController(model.Object);

            // Assert
            var result = controller.DeleteTask(todoTask.Id);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void DeleteListTest_ReturnNotFoundList()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            TodoTask todoTask = null;
            int todoTaskId = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(todoTaskId))
                .Returns(Task.FromResult(user));
            #endregion

            // Act
            controller = new TodoTasksController(model.Object);

            // Assert
            var result = controller.DeleteTask(todoTaskId);
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void CreateTaskTest_ReturnCreatedTask()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            CreateTodoTaskDTO createToDoTaskDTO = new CreateTodoTaskDTO
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
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(
                createToDoTaskDTO.ToDoListId, user.Id)).Returns(Task.FromResult(todoList));
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
        public void CreateTaskTest_ReturnUserNotFound()
        {
            #region Arrange
            User user = null;
            CreateTodoTaskDTO createToDoTaskDTO = new CreateTodoTaskDTO
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
            model.Setup(repo => repo.GetUserById(userId)).Returns(Task.FromResult(user));
            #endregion

            // Act
            controller = new TodoTasksController(model.Object);
            var result = controller.CreateTask(createToDoTaskDTO);

            // Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void CreateTaskTest_ReturnTodoListNotFound()
        {
            #region Arrange
            User user = new User(1, "Name1", "Email1", "Pass1");
            CreateTodoTaskDTO createToDoTaskDTO = new CreateTodoTaskDTO
            {
                ToDoListId = 1,
                Title = "Title1",
                Description = "Description1"
            };
            TodoList todoList = null;
            TodoTask createdTodoTask = new TodoTask(createToDoTaskDTO);

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(
                createToDoTaskDTO.ToDoListId, user.Id)).Returns(Task.FromResult(todoList));
            #endregion

            // Act
            controller = new TodoTasksController(model.Object);
            var result = controller.CreateTask(createToDoTaskDTO);

            // Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void CreateTaskTest_ReturnBadRequestEmptyField()
        {
            // Arrange 
            User user = new User(1, "Name1", "Email1", "Pass1");
            CreateTodoTaskDTO createToDoTaskDTO = new CreateTodoTaskDTO
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
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoListByListIdAndUserId(
                createToDoTaskDTO.ToDoListId, user.Id)).Returns(Task.FromResult(todoList));
            model.Setup(repo => repo.AddTodoTask(createdTodoTask));

            // Act
            controller = new TodoTasksController(model.Object);
            var result = controller.CreateTask(createToDoTaskDTO);

            // Assert
            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void CreateTaskTest_ReturnBadRequestInvalidModel()
        {
            // Arrange 
            User user = new User(1, "Name1", "Email1", "Pass1");
            CreateTodoTaskDTO createToDoTaskDTO = null;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();

            // Act
            controller = new TodoTasksController(model.Object);
            var result = controller.CreateTask(createToDoTaskDTO);

            // Assert
            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void SetTaskStatusTest_ReturnCorrect()
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
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoTaskById(todoList.Id))
                .Returns(Task.FromResult(todoTask));
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
        public void SetTaskStatusTest_ReturnTodoTaskNotFound()
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
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoTaskById(2))
                .Returns(Task.FromResult(todoTask));
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
        public void SetTaskStatusTest_ReturnUserNotFound()
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
            model.Setup(repo => repo.GetUserById(userId)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoTaskById(todoList.Id))
                .Returns(Task.FromResult(todoTask));
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
        public void SetTaskStatusTest_ReturnBadRequestModelIsInvalid()
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
        public void UpdateTaskTest_ReturnCorrectUpdated()
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

            UpdateTodoTaskDTO updateToDoTaskDTO = new UpdateTodoTaskDTO
            {
                TaskId = todoTask.Id,
                Description = "New Description",
                Title = "New title"
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoTaskById(updateToDoTaskDTO.TaskId))
                .Returns(Task.FromResult(todoTask));
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
        public void UpdateTaskTest_ReturnTodoTaskNotFound()
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

            UpdateTodoTaskDTO updateToDoTaskDTO = new UpdateTodoTaskDTO
            {
                TaskId = 1,
                Description = "New Description",
                Title = "New title"
            };

            TodoTask todoTask = null;
            int todoTaskId = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoTaskById(todoTaskId))
                .Returns(Task.FromResult(todoTask));
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
        public void UpdateTaskTest_ReturnUserNotFound()
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

            UpdateTodoTaskDTO updateToDoTaskDTO = new UpdateTodoTaskDTO
            {
                TaskId = todoTask.Id,
                Description = "New Description",
                Title = "New title"
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(userId)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoTaskById(updateToDoTaskDTO.TaskId))
                .Returns(Task.FromResult(todoTask));
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
        public void UpdateTaskTest_ReturnBadRequestNotValidModelState()
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

            UpdateTodoTaskDTO updateToDoTaskDTO = null;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoTaskById(todoTask.Id))
                .Returns(Task.FromResult(todoTask));
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
        public void UpdateTaskTest_ReturnBadRequestEmptyFields()
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

            UpdateTodoTaskDTO updateToDoTaskDTO = new UpdateTodoTaskDTO
            {
                TaskId = 0,
                Description = "",
                Title = ""
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetUserById(user.Id)).Returns(Task.FromResult(user));
            model.Setup(repo => repo.GetTodoTaskById(updateToDoTaskDTO.TaskId))
                .Returns(Task.FromResult(todoTask));
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
        public void GetTodoTaskTest_ReturnCorrectOk()
        {
            #region Arrange
            TodoTask todoTask = new TodoTask
            {
                Id = 1,
                Description = "Description1",
                Title = "List1",
                ToDoListId = 1,
                TaskStatus = TodoTask.Status.AWAIT
            };

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetTodoTaskById(todoTask.Id))
                .Returns(Task.FromResult(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.GetTodoTask(todoTask.Id);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            TodoTask taskFromServer = okObjectResult.Value as TodoTask;
            Assert.Equal(todoTask.Id, taskFromServer.Id);
            #endregion
        }

        [Fact]
        public void GetTodoTaskTest_ReturnBadRequestNegativeId()
        {
            #region Arrange
            int negativeId = -1;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.GetTodoTask(negativeId);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            #endregion
        }

        [Fact]
        public void GetTodoTaskTest_ReturnNotFoundTodoTask()
        {
            #region Arrange
            TodoTask todoTask = null;
            int todoTaskId = 2;

            Extensions.Extensions.IsUnitTest = true;

            model = new Mock<IRepository>();
            model.Setup(repo => repo.GetTodoTaskById(todoTaskId))
                .Returns(Task.FromResult(todoTask));
            #endregion

            #region Act
            controller = new TodoTasksController(model.Object);
            var result = controller.GetTodoTask(todoTaskId);
            #endregion

            #region Assert
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            #endregion
        }
    }
}