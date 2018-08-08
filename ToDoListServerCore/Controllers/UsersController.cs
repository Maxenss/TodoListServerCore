﻿using System;
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


        // DELETE: api/Users/
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = User.GetUserId();

            var user = _context.GetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            _context.RemoveUser(user);

            return Ok(user);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id) {
            if (!ModelState.IsValid)
                return BadRequest("Model state is not valid");

            User user = _context.GetUserById(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }
    }
}