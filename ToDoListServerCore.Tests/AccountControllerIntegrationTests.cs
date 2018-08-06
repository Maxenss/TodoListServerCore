using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoListServerCore.Models.DTO;
using Xunit;

namespace ToDoListServerCore.Tests
{
   public class AccountControllerIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public AccountControllerIntegrationTests()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(@"C:\Work\Projects\Some Work\ToDoListServerCore\ToDoListServerCore")
                .UseEnvironment("Development")
                .UseStartup<ToDoListServerCore.Startup>()
                .UseApplicationInsights();

            // Arrange
            _server = new TestServer(builder);
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task SignIn_Post_Test()
        {
            // Arrange
            var signInDto = new SignInDTO
            {
                Email = "test@gmail.com",
                Password = "1234"
            };
            var content = JsonConvert.SerializeObject(signInDto);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Account/signin", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var person = JsonConvert.DeserializeObject<UserDTO>(responseString);
            Assert.NotNull(person.Token);
        }
    }
}