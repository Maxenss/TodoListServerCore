﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoListServerCore.Models;
using ToDoListServerCore.Models.DTO;
using Xunit;

namespace ToDoListServerCore.Tests
{
    public class ToDoListServerIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ToDoListServerIntegrationTests()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("Development")
                .UseStartup<ToDoListServerCore.Startup>()
                .UseApplicationInsights();

            // Arrange
            _server = new TestServer(builder);
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task System_Test()
        {
            // Sign Up Test
            User user = await SignUpPost();

            // Sign In Test
            UserDTO userDTO = await SignInPost();
            var token = userDTO.Token;

            // Set authorization header for authorization
            _client.DefaultRequestHeaders.Authorization =
               new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Create Todo List Test
            var todoList = await CreateToDoList("New List 1");
            var todoList2 = await CreateToDoList("New List2");

            // Create Todo Task Test
            var todoTask1 = await CreateToDoTask(todoList.Id, "Title1", "Description1");
            var todoTask2 = await CreateToDoTask(todoList.Id, "Title2", "Description2");
            var todoTask3 = await CreateToDoTask(todoList.Id, "Title3", "Description3");

            // Set Todo Task status Test 
            await SetTodoTaskStatus(todoTask1.Id, TodoTask.Status.DONE);
            await SetTodoTaskStatus(todoTask2.Id, TodoTask.Status.CANCELED);
            await SetTodoTaskStatus(todoTask2.Id, TodoTask.Status.AWAIT);

            // Update Todo Task Test
            TodoTaskDTO updatedToDoTask = await UpdateTodoTask(todoTask1, todoList2);

            // Get Todo Lists For User Test
            await GetListsForUser(2);

            // Delete Todo List Test
            await DeleteTodoList(todoList2.Id);

            // Get Todo Lists For User Test
            await GetListsForUser(1);

            // Set List Title Test
            await SetListTitle(todoList.Id, "Updated Title");
        }

        private async Task<User> SignUpPost()
        {
            // Arrange
            var signUpDTO = new SignUpDTO
            {
                Email = "test3200@gmail.com",
                Password = "123456",
                Name = "User123"
            };
            var content = JsonConvert.SerializeObject(signUpDTO);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Account/signup", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(responseString);
            Assert.NotNull(user);

            return user;
        }

        private async Task<UserDTO> SignInPost()
        {
            // Arrange
            var signInDto = new SignInDTO
            {
                Email = "test3200@gmail.com",
                Password = "123456"
            };
            var content = JsonConvert.SerializeObject(signInDto);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Account/signin", stringContent);

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            var userDto = JsonConvert.DeserializeObject<UserDTO>(responseString);
            Assert.NotNull(userDto);

            return userDto;
        }

        private async Task<TodoList> CreateToDoList(string title)
        {
            // Arrange
            CreateListDTO createListDTO = new CreateListDTO
            {
                Title = title,
            };
            var content = JsonConvert.SerializeObject(createListDTO);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/TodoLists/", stringContent);

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            var todoList = JsonConvert.DeserializeObject<TodoList>(responseString);
            Assert.NotNull(todoList);
            return todoList;
        }

        private async Task<TodoList> SetListTitle(int listId, string title)
        {
            // Act
            string url = "/api/TodoLists/setlisttitle?"
                     + "listId=" + listId + "&title=" + title;
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url);
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            TodoList todoList =
                JsonConvert.DeserializeObject<TodoList>(responseString);
            Assert.NotNull(todoList);
            Assert.Equal(title, todoList.Title);

            return todoList;
        }

        private async Task<TodoTaskDTO> CreateToDoTask(int listId
            , string title, string description)
        {
            // Arrange
            CreateToDoTaskDTO createToDoTaskDTO1 = new CreateToDoTaskDTO
            {
                ToDoListId = listId,
                Title = title,
                Description = description
            };

            var content = JsonConvert.SerializeObject(createToDoTaskDTO1);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/TodoTasks/", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            TodoTaskDTO todoTaskDTO = JsonConvert.DeserializeObject<TodoTaskDTO>(responseString);
            Assert.NotNull(todoTaskDTO);

            return todoTaskDTO;
        }

        private async Task<TodoTaskDTO> SetTodoTaskStatus(int todoTaskId
            , TodoTask.Status taskStatus)
        {
            // Arrange
            string url = "/api/TodoTasks/" + todoTaskId + "/setstatus/" + taskStatus;

            // Act
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url);
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            TodoTaskDTO updatedTodoTaskDTO
               = JsonConvert.DeserializeObject<TodoTaskDTO>(responseString);
            Assert.NotNull(updatedTodoTaskDTO);
            Assert.Equal(updatedTodoTaskDTO.TaskStatus, taskStatus);
            Assert.Equal(updatedTodoTaskDTO.Id, todoTaskId);

            return updatedTodoTaskDTO;
        }

        private async Task<TodoTaskDTO> UpdateTodoTask(TodoTaskDTO todoTaskDTO
            , TodoList todoList)
        {
            // Arrange
            UpdateToDoTaskDTO updateToDoTaskDTO = new UpdateToDoTaskDTO
            {
                TaskId = todoTaskDTO.Id,
                ToDoListId = todoTaskDTO.ToDoListId,
                Title = todoTaskDTO.Title,
                Description = todoTaskDTO.Description
            };

            updateToDoTaskDTO.Description = "New Description";
            updateToDoTaskDTO.ToDoListId = todoList.Id;
            updateToDoTaskDTO.Title = "New Title";

            var content = JsonConvert.SerializeObject(updateToDoTaskDTO);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/TodoTasks/", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            TodoTaskDTO updatedTodoTaskDTO
                = JsonConvert.DeserializeObject<TodoTaskDTO>(responseString);
            Assert.NotNull(updatedTodoTaskDTO);
            Assert.Equal(updateToDoTaskDTO.TaskId, updatedTodoTaskDTO.Id);
            Assert.Equal(updateToDoTaskDTO.Title, updatedTodoTaskDTO.Title);
            Assert.Equal(updateToDoTaskDTO.Description, updatedTodoTaskDTO.Description);
            Assert.Equal(updateToDoTaskDTO.ToDoListId, updatedTodoTaskDTO.ToDoListId);

            return updatedTodoTaskDTO;
        }

        private async Task<List<TodoList>> GetListsForUser(int countOfLists)
        {
            // Act
            var response = await _client.GetAsync("/api/TodoLists/");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<TodoList> todoLists
                = JsonConvert.DeserializeObject<List<TodoList>>(responseString);
            Assert.NotNull(todoLists);
            Assert.Equal(todoLists.Count, countOfLists);

            return todoLists;
        }

        private async Task DeleteUser()
        {
            var response = await _client.DeleteAsync("/api/Users/");

            response.EnsureSuccessStatusCode();
        }

        private async Task DeleteTodoList(int todoListId)
        {
            var response = await _client.DeleteAsync("/api/todolists/" + todoListId);

            response.EnsureSuccessStatusCode();
        }
    }
}