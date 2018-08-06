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
        DBContext _context;

        public TodoTasksController(DBContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("{taskListId}")]
        public async Task<IActionResult> GetTasks(int taskListId)
        {
            if (ModelState.IsValid)
            {
                int userId = this.User.GetUserId();

                User user = _context.GetUserById(userId);

                if (user == null)
                    return NotFound("User with this id not found.");

                TodoList todoList =  _context.GetTodoListByListIdAndUserId(taskListId, userId);

                if (todoList == null)
                    return NotFound("Todo list not found.");

                return Ok(todoList);
            }

            return BadRequest("Model state is not valid.");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateToDoTaskDTO createToDoTaskDTO)
        {
            if (ModelState.IsValid)
            {
                int userId = User.GetUserId();

                User user =  _context.GetUserById(userId);

                if (user == null)
                    return NotFound("User with this id not found.");

                TodoList existTodoList = 
                    _context.GetTodoListByListIdAndUserId(createToDoTaskDTO.ToDoListId, userId);

                if (existTodoList == null)
                    return NotFound("This user not have todo list with this id");

                TodoTask todoTask = new TodoTask(createToDoTaskDTO.ToDoListId
                    , createToDoTaskDTO.Title, createToDoTaskDTO.Description);

                todoTask.TaskStatus = TodoTask.Status.AWAIT;

                _context.AddTodoTask(todoTask);

                string webRootPath = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                string objectLocation = webRootPath + "/" + "api/TodoTasks/" + todoTask.Id.ToString();

                return Created(objectLocation, new TodoTaskDTO(todoTask));
            }

            return BadRequest("Model state is not valid.");
        }

        [Authorize]
        [HttpPatch("{taskId}/setstatus/{status}")]
        public async Task<IActionResult> SetTaskStatus(int taskId, Status status)
        {
            if (ModelState.IsValid)
            {
                int userId = this.User.GetUserId();

                User user =  _context.GetUserById(userId);

                if (user == null)
                    return NotFound("User with this id not found.");

                TodoTask todoTask = _context.GetTodoTaskById(taskId); 

                if (todoTask == null)
                    return BadRequest("Todo task with this id not found.");

                if (user.TodoLists.SingleOrDefault(l => l.Id == todoTask.Id) == null)
                    return BadRequest("Todo task with this id not found.");

                todoTask.TaskStatus = status;

                _context.UpdateTodoTask(todoTask);

                return Ok(new TodoTaskDTO(todoTask));
            }

            return BadRequest("Model state is not valid.");
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateToDoTaskDTO updateToDoTaskDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid.");
            }

            int userId = this.User.GetUserId();

                User user = _context.GetUserById(userId);

                if (user == null)
                    return NotFound("User with this id not found.");

                TodoTask todoTask = _context.GetTodoTaskById(updateToDoTaskDTO.TaskId);

                if (todoTask == null)
                    return BadRequest("Todo task with this id not found.");

                if (user.TodoLists.SingleOrDefault(l => l.Id == todoTask.Id) == null)
                    return BadRequest("Todo task with this id not found.");

                todoTask.Description = updateToDoTaskDTO.Description;
                todoTask.Title = updateToDoTaskDTO.Title;
                todoTask.ToDoListId = updateToDoTaskDTO.ToDoListId;

                _context.UpdateTodoTask(todoTask);

                return Ok(todoTask);
        }

        //   [HttpGet("taskstatuses")]
        //   public async Task<IActionResult> GetStatusEnum()
        //   {
        //       TodoTask.Status statuses = new TodoTask.Status();
        //
        //       return Ok(Json(statuses));
        //   }
    }
}