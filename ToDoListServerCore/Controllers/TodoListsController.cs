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

        /// <summary>
        /// Method for create TodoList.
        /// </summary>
        /// <param name="createListDTO">DTO for creating TodoList</param>
        /// <returns>Created TodoList</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody] CreateListDTO createListDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            if (createListDTO == null ||
                createListDTO.Title == String.Empty)
                return BadRequest("Error: Title is empty.");

            string title = createListDTO.Title;

            if (title == null || title.Length == 0)
                return BadRequest("Error: Title cannot to be empty.");

            var userId = this.User.GetUserId();

            User user = await _context.GetUserById(userId);

            if (user == null)
                return NotFound("Error: User not found.");

            TodoList existToDoList = await _context
                .GetTodoListByTitleAndUserId(title, userId);

            if (existToDoList != null)
                return BadRequest("Error: This Todo List already exist.");

            TodoList todoList = new TodoList(userId, title);
            _context.AddTodoList(todoList);

            if (Extensions.Extensions.IsUnitTest)
                return Created("localhost", todoList);

            string webRootPath = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            string objectLocation = webRootPath + "/" + "api/todolists/" + todoList.Id.ToString();
            return Created(objectLocation, todoList);
        }

        /// <summary>
        /// Method for delete TodoList.
        /// </summary>
        /// <param name="listId">TodoList ID for delete</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{listId}")]
        public async Task<IActionResult> DeleteList(int listId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            if (listId < 1)
                return BadRequest("Error: List id cannot be negative.");

            var userId = this.User.GetUserId();

            User user = await _context.GetUserById(userId);

            if (user == null)
                return NotFound("Error: User with this id not found.");

            TodoList todoList = await _context
                .GetTodoListByListIdAndUserId(listId, userId);

            if (todoList == null)
                return NotFound("Error: Todo List with this id not found.");

            _context.RemoveTodoList(todoList);

            return Ok("Todo List has been deleted.");
        }

        /// <summary>
        /// Method for set title of TodoList.
        /// </summary>
        /// <param name="listId">TodoList ID for delete</param>
        /// <param name="title">New title for TodoList</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("setlisttitle")]
        public async Task<IActionResult> SetListTitle(int listId, string title)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            if (listId < 1)
                return BadRequest("Error: List id cannot be negative.");

            if (title == null || title.Length == 0)
                return BadRequest("Error: Title cannot be empty.");

            var userId = User.GetUserId();

            User user = await _context.GetUserById(userId);

            if (user == null)
                return NotFound("Error: User with this id not found.");

            TodoList todoList = await _context
                .GetTodoListByListIdAndUserId(listId, userId);

            if (todoList == null)
                return NotFound("Error: Todo List with this id not found.");

            todoList.Title = title;

            _context.UpdateTodoList(todoList);

            return Ok(todoList);
        }

        /// <summary>
        /// Method for get TaskList by id.
        /// </summary>
        /// <param name="listId">TodoList ID for get TodoList</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{listId}")]
        public async Task<IActionResult> GetTaskList(int listId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            if (listId < 1)
                return BadRequest("Error: List id cannot be negative.");

            var userId = User.GetUserId();

            User user = await _context.GetUserById(userId);

            if (user == null)
                return NotFound("Error: User with this id not found.");

            TodoList todoList = await _context
                .GetTodoListByListIdAndUserId(listId, userId);

            if (todoList == null)
                return NotFound("Error: Todo List with this id not found.");

            return Ok(todoList);
        }

        /// <summary>
        /// Method for get TaskLists for User
        /// </summary>
        /// <returns>List with TodoLists</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetListsForUser()
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            int userId = this.User.GetUserId();
            User user = await _context.GetUserById(userId);

            if (user == null)
                return NotFound("Errod: User with this id not found.");

            List<TodoList> todoLists = _context.GetTodoListsByUserId(userId);

            return Ok(todoLists);
        }

    }
}