using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListServerCore;
using ToDoListServerCore.DB;
using ToDoListServerCore.Extensions;

namespace ToDoListServerCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IRepository _context;

        public UsersController(IRepository context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for delete user.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            int userId = User.GetUserId();

            var user = await _context.GetUserById(userId);
            if (user == null)
                return NotFound("Error: User not found.");

            _context.RemoveUser(user);

            return Ok("User has been deleted.");
        }

        /// <summary>
        /// Method for get user.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid");

            User user = await _context.GetUserById(id);

            if (user == null)
                return NotFound("Error: User not found");

            return Ok(user);
        }
    }
}