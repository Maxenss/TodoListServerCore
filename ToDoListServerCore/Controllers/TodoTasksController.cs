﻿using System;
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

                TodoList todoList = _context.GetTodoListByListIdAndUserId(taskListId, userId);

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
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid.");
            }

            if (createToDoTaskDTO == null)
                return BadRequest("Model state is not valid.");

            if (createToDoTaskDTO.ToDoListId < 1)
                return BadRequest("Todo list id cannot be negative.");

            if (createToDoTaskDTO.Title == null || createToDoTaskDTO.Title == String.Empty)
                return BadRequest("Todo task title cannot be empty.");

            if (createToDoTaskDTO.Description == null 
                || createToDoTaskDTO.Description == String.Empty)
                return BadRequest("Todo task description cannot be empty.");

            int userId = User.GetUserId();

            User user = _context.GetUserById(userId);

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

            if (Extensions.Extensions.IsUnitTest)
                return Created("localhost", new TodoTaskDTO(todoTask));

            string webRootPath = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            string objectLocation = webRootPath + "/" + "api/TodoTasks/" + todoTask.Id.ToString();

            return Created(objectLocation, new TodoTaskDTO(todoTask));
        }

        [Authorize]
        [HttpPatch("{taskId}/setstatus/{status}")]
        public async Task<IActionResult> SetTaskStatus(int taskId, Status status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid.");
            }

            if (taskId < 1) {
                return BadRequest("Task id cannot be negative.");
            }

            int userId = this.User.GetUserId();

                User user = _context.GetUserById(userId);

                if (user == null)
                    return NotFound("User with this id not found.");

                TodoTask todoTask = _context.GetTodoTaskById(taskId);

                if (todoTask == null)
                    return NotFound("Todo task with this id not found.");

                if (user.TodoLists.SingleOrDefault(l => l.Id == todoTask.ToDoListId) == null)
                    return NotFound("Todo list with this id not found.");

                todoTask.TaskStatus = status;

                _context.UpdateTodoTask(todoTask);

                return Ok(new TodoTaskDTO(todoTask));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateToDoTaskDTO updateToDoTaskDTO)
        {
            if (!ModelState.IsValid || updateToDoTaskDTO == null)
            {
                return BadRequest("Model state is not valid.");
            }

            if (updateToDoTaskDTO.TaskId < 1)
                return BadRequest("Task id cannot be an empty");

            if (updateToDoTaskDTO.Title == null || updateToDoTaskDTO.Title.Length == 0) 
                return BadRequest("Task title cannot be an empty");

            if (updateToDoTaskDTO.Description == null || updateToDoTaskDTO.Description.Length == 0)
                return BadRequest("Task description cannot be an empty");

            int userId = this.User.GetUserId();

            User user = _context.GetUserById(userId);

            if (user == null)
                return NotFound("User with this id not found.");

            TodoTask todoTask = _context.GetTodoTaskById(updateToDoTaskDTO.TaskId);

            if (todoTask == null)
                return NotFound("Todo task with this id not found.");

            if (user.TodoLists.SingleOrDefault(l => l.Id == todoTask.ToDoListId) == null)
                return NotFound("Todo list with this id not found.");

            todoTask.Description = updateToDoTaskDTO.Description;
            todoTask.Title = updateToDoTaskDTO.Title;
            todoTask.ToDoListId = updateToDoTaskDTO.ToDoListId;

            _context.UpdateTodoTask(todoTask);

            return Ok(todoTask);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model state is not valid.");

            int userId = User.GetUserId();

            User user = _context.GetUserById(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            TodoTask todoTask = _context.GetTodoTaskById(id);

            if (todoTask == null)
                return NotFound();

            _context.RemoveTodoTask(todoTask);

            return Ok("Todo Task has been deleted.");
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