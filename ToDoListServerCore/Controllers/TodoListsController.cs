using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListServerCore.DB;
using ToDoListServerCore.Extensions;
using ToDoListServerCore.Models;
using ToDoListServerCore.Models.DTO;

namespace ToDoListServerCore.Controllers
{
    [Produces("application/json")]
    [Route("api/TodoLists")]
    public class TodoListsController : Controller
    {
        private readonly IRepository _context;


        public TodoListsController(IRepository context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody] CreateListDTO createListDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model state is not valid.");

            if (createListDTO == null ||
                createListDTO.Title == String.Empty)
                return BadRequest("Title is empty");

            string title = createListDTO.Title;

            if (title == null || title.Length == 0) return BadRequest("Title cannot to be empty");

            var userId = this.User.GetUserId();

            User user = _context.GetUserById(userId);

            if (user == null) return NotFound();

            TodoList existToDoList = _context.GetTodoListByTitleAndUserId(title, userId);

            if (existToDoList != null)
                return BadRequest("This Todo List already exist");

            TodoList todoList = new TodoList(userId, title);
            _context.AddTodoList(todoList);

            if (Extensions.Extensions.IsUnitTest)
                return Created("localhost", todoList);

            string webRootPath = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            string objectLocation = webRootPath + "/" + "api/users/" + todoList.Id.ToString();
            return Created(objectLocation, todoList);
        }

        [Authorize]
        [HttpDelete("{listId}")]
        public async Task<IActionResult> DeleteList(int listId)
        {
            if (ModelState.IsValid)
            {
                if (listId < 1)
                    return BadRequest();

                var userId = this.User.GetUserId();

                User user = _context.GetUserById(userId);

                if (user == null) return NotFound("User with this id not found");

                TodoList todoList = _context.GetTodoListByListIdAndUserId(listId, userId);

                if (todoList == null) return NotFound("Todo List with this id not found");

                _context.RemoveTodoList(todoList);

                return Ok("Todo List has been deleted");
            }

            return BadRequest();
        }

        [Authorize]
        [HttpPatch("setlisttitle")]
        public async Task<IActionResult> SetListTitle(int listId, string title)
        {
            if (ModelState.IsValid)
            {
                if (listId < 1)
                    return BadRequest();
                if (title == null || title.Length == 0)
                    return BadRequest();

                var userId = User.GetUserId();

                User user = _context.GetUserById(userId);

                if (user == null) return NotFound("User with this id not found");

                TodoList todoList = _context.GetTodoListByListIdAndUserId(listId, userId);

                if (todoList == null) return NotFound("Todo List with this id not found");

                todoList.Title = title;

                _context.UpdateTodoList(todoList);

                return Ok(todoList);
            }

            return BadRequest();
        }

        [Authorize]
        [HttpGet("{listId}")]
        public async Task<IActionResult> GetTaskList(int listId)
        {
            if (ModelState.IsValid)
            {
                if (listId < 1)
                    return BadRequest("Negative id is not valid");

                var userId = User.GetUserId();

                User user = _context.GetUserById(userId);

                if (user == null) return NotFound("User with this id not found");

                TodoList todoList = _context.GetTodoListByListIdAndUserId(listId, userId);

                if (todoList == null) return NotFound("Todo List with this id not found");

                return Ok(todoList);
            }

            return BadRequest();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetListsForUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid.");
            }

            int userId = this.User.GetUserId();
            User user = _context.GetUserById(userId);

            if (user == null) return NotFound("User with this id not found");

            List<TodoList> todoLists = _context.GetTodoListsByUserId(userId);

            return Ok(todoLists);
        }

    }
}