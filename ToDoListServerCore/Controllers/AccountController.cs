using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDoListServerCore.DB;
using ToDoListServerCore.Models.DTO;

namespace ToDoListServerCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IRepository _context;
        private readonly IConfiguration _configuration;

        public AccountController(DBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // signup
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid.");
            }

            User existUser = _context.GetUsers().SingleOrDefault(u => u.Email == signUpDTO.Email);

            if (existUser != null) { return BadRequest("User with this email already exist."); }

            User user = new User(signUpDTO.Name, signUpDTO.Email, signUpDTO.Password);

            _context.AddUser(user);

            return Created("User Created", user);
        }

        // signin
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid.");
            }

            User user = _context.GetUserByEmailAndPassword(signInDTO.Email, signInDTO.Password);

            if (user == null)
                return NotFound("Not correct email or password.");

            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, "User")
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds);

            // Write token to memory
            string resToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Create DTO for response
            UserDTO userDTO = new UserDTO(resToken, DateTime.Now.AddHours(24), DateTime.Now, user);

            return Ok(userDTO);
        }
    }
}