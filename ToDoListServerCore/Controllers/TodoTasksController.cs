using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListServerCore.DB;
using ToDoListServerCore.Extensions;
using ToDoListServerCore.Models;
using ToDoListServerCore.Models.DTO;
using static ToDoListServerCore.Models.TodoTask;

namespace ToDoListServerCore.Controllers
{
    [Produces("application/json")]
    [Route("api/TodoTasks")]
    public class TodoTasksController : Controller
    {
        private readonly IRepository _context;

        public TodoTasksController(IRepository context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for create TodoList.
        /// </summary>
        /// <param name="createToDoTaskDTO">DTO for creating TodoList</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTodoTaskDTO createToDoTaskDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            if (createToDoTaskDTO == null)
                return BadRequest("Error: Model state is not valid.");

            if (createToDoTaskDTO.ToDoListId < 1)
                return BadRequest("Error: Todo list id cannot be negative.");

            if (createToDoTaskDTO.Title == null || createToDoTaskDTO.Title == String.Empty)
                return BadRequest("Error: Todo task title cannot be empty.");

            if (createToDoTaskDTO.Description == null
                || createToDoTaskDTO.Description == String.Empty)
                return BadRequest("Error: Todo task description cannot be empty.");

            int userId = User.GetUserId();

            User user = await _context.GetUserById(userId);

            if (user == null)
                return NotFound("Error: User with this id not found.");

            TodoList existTodoList = await _context
                .GetTodoListByListIdAndUserId(createToDoTaskDTO.ToDoListId, userId);

            if (existTodoList == null)
                return NotFound("Error: This user not have todo list with this id");

            TodoTask todoTask = new TodoTask(createToDoTaskDTO.ToDoListId
                , createToDoTaskDTO.Title, createToDoTaskDTO.Description);

            todoTask.TaskStatus = TodoTask.Status.AWAIT;

            _context.AddTodoTask(todoTask);

            if (Extensions.Extensions.IsUnitTest)
                return Created("localhost", new TodoTaskDTO(todoTask));

            string webRootPath = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            string objectLocation = webRootPath + "/" + "api/TodoTasks/" + todoTask.Id.ToString();

            return Created(objectLocation, new TodoTaskDTO(todoTask));
        }

        /// <summary>
        /// Method for changing the status of a task.
        /// </summary>
        /// <param name="taskId">TodoTask's ID for set status.</param>
        /// <param name="status">New status.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("{taskId}/setstatus/{status}")]
        public async Task<IActionResult> SetTaskStatus(int taskId, Status status)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            if (taskId < 1)
                return BadRequest("Error: Task id cannot be negative.");

            int userId = this.User.GetUserId();

            User user = await _context.GetUserById(userId);

            if (user == null)
                return NotFound("Error: User with this id not found.");

            TodoTask todoTask = await _context.GetTodoTaskById(taskId);

            if (todoTask == null)
                return NotFound("Error: Todo task with this id not found.");

            if (user.TodoLists.SingleOrDefault(l => l.Id == todoTask.ToDoListId) == null)
                return NotFound("Error: Todo list with this id not found.");

            todoTask.TaskStatus = status;

            _context.UpdateTodoTask(todoTask);

            return Ok(new TodoTaskDTO(todoTask));
        }

        /// <summary>
        /// Method for update TodoTask.
        /// </summary>
        /// <param name="updateToDoTaskDTO">DTO for change TodoTask</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTodoTaskDTO updateToDoTaskDTO)
        {
            if (!ModelState.IsValid || updateToDoTaskDTO == null)
                return BadRequest("Error: Model state is not valid.");

            if (updateToDoTaskDTO.TaskId < 1)
                return BadRequest("Error: Task id cannot be an empty.");

            if (updateToDoTaskDTO.Title == null || updateToDoTaskDTO.Title.Length == 0)
                return BadRequest("Error: Task title cannot be an empty.");

            if (updateToDoTaskDTO.Description == null || updateToDoTaskDTO.Description.Length == 0)
                return BadRequest("Error: Task description cannot be an empty.");

            int userId = this.User.GetUserId();

            User user = await _context.GetUserById(userId);

            if (user == null)
                return NotFound("Error: User with this id not found.");

            TodoTask todoTask = await _context.GetTodoTaskById(updateToDoTaskDTO.TaskId);

            if (todoTask == null)
                return NotFound("Error: Todo task with this id not found.");

            if (user.TodoLists.SingleOrDefault(l => l.Id == todoTask.ToDoListId) == null)
                return NotFound("Error: Todo list with this id not found.");

            todoTask.Description = updateToDoTaskDTO.Description;
            todoTask.Title = updateToDoTaskDTO.Title;
            todoTask.ToDoListId = updateToDoTaskDTO.ToDoListId;

            _context.UpdateTodoTask(todoTask);

            return Ok(todoTask);
        }

        /// <summary>
        /// Method for delete TodoTask.
        /// </summary>
        /// <param name="id">TodoTask's ID for delete</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            int userId = User.GetUserId();

            User user = await _context.GetUserById(userId);

            if (user == null)
                return NotFound("Error: User not found.");

            TodoTask todoTask = await _context.GetTodoTaskById(id);

            if (todoTask == null)
                return NotFound("Error: Todo Task with this id not found.");

            _context.RemoveTodoTask(todoTask);

            return Ok("Todo Task has been deleted.");
        }

        /// <summary>
        /// Method for get TodoTask by id.
        /// </summary>
        /// <param name="id">TodoTask's ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoTask(int id)
        {
            if (!ModelState.IsValid || id < 1)
                return BadRequest("Error: Model state is not valid.");

            TodoTask todoTask = await _context.GetTodoTaskById(id);

            if (todoTask == null)
                return NotFound("Error: Todo Task with this id not found.");

            return Ok(todoTask);
        }
    }
}