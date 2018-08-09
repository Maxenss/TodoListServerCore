using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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

        public AccountController(IRepository context)
        {
            _context = context;

            // Set up configuration sources.
            IConfigurationBuilder builder = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json");

            builder.AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        /// <summary>
        /// Method for Sign Up.
        /// </summary>
        /// <param name="signUpDTO">DTO with data for sign up.</param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Error: Model state is not valid.");

            // Email validation
            if (signUpDTO.Email == null || signUpDTO.Email == String.Empty)
                return BadRequest("Error: Empty Email.");

            if (!IsValidEmail(signUpDTO.Email))
                return BadRequest("Error: Email is not valid.");

            // Password validatition
            if (signUpDTO.Password == null || signUpDTO.Password == String.Empty)
                return BadRequest("Error: Empty Password");
            if (signUpDTO.Password.Length < 6)
                return BadRequest("Error: Short Password");

            // Name validation
            if (signUpDTO.Name == null || signUpDTO.Name == String.Empty)
                return BadRequest("Error: Empty Name");

            User existUser = await _context.GetUserByEmail(signUpDTO.Email);

            if (existUser != null)
                return BadRequest("Error: User with this email already exist.");

            User user = new User(signUpDTO.Name, signUpDTO.Email, signUpDTO.Password);
            user.TodoLists = new List<Models.TodoList>();

            _context.AddUser(user);

            if (Extensions.Extensions.IsUnitTest)
                return Created("localhost", user);

            // Get URL patch of object
            string webRootPath = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            string objectLocation = webRootPath + "/" + "api/TodoLists/" + user.Id.ToString();

            return Created(objectLocation, user);
        }

        /// <summary>
        /// Method for Sign In.
        /// </summary>
        /// <param name="signInDTO">DTO with data for sign in.</param>
        /// <returns></returns>
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
        {
            if (!ModelState.IsValid || signInDTO == null)
                return BadRequest("Error: Model state is not valid.");

            // Email validation
            if (signInDTO.Email == null || signInDTO.Email == String.Empty)
                return BadRequest("Error: Empty Email.");

            if (!IsValidEmail(signInDTO.Email))
                return BadRequest("Error: Email is not valid.");

            // Password validatition
            if (signInDTO.Password == null || signInDTO.Password == String.Empty)
                return BadRequest("Error: Empty Password.");

            User user = await _context
                .GetUserByEmailAndPassword(signInDTO.Email, signInDTO.Password);

            if (user == null)
                return NotFound("Error: Not correct email or password.");

            // Create claims with data 
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

        /// <summary>
        /// Method for validation Email.
        /// </summary>
        /// <param name="email">Email in string representation.</param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                System.Net.Mail.MailAddress addr
                    = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}