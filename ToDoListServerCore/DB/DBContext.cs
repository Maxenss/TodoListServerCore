﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListServerCore.Models;

namespace ToDoListServerCore.DB
{
    public class DBContext : DbContext, IRepository
    {
        public DBContext(DbContextOptions<DBContext> options) :
         base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<TodoList> TodoLists { get; set; }

        public DbSet<TodoTask> TodoTasks { get; set; }

        public IEnumerable<TodoList> GetToDoLists()
        {
            return TodoLists;
        }

        public IEnumerable<TodoTask> GetToDoTasks()
        {
            return TodoTasks;
        }

        public IEnumerable<User> GetUsers()
        {
            return Users;
        }

        public User GetUserByEmail(string email)
        {
            return this.Users.Include(t => t.TodoLists)
                .SingleOrDefault(u => u.Email == email);
        }

        public User GetUserById(int id)
        {
            return this.Users.Include(t => t.TodoLists)
                .SingleOrDefault(u => u.Id == id);
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            User user = this.Users.Include(t => t.TodoLists).SingleOrDefault(
               u => u.Email == email && u.Password == password);

            return user;
        }

        public TodoList GetTodoListByTitleAndUserId(string title, int userId)
        {
            return this.TodoLists.SingleOrDefault(l => l.UserId == userId && l.Title == title);
        }

        public TodoList GetTodoListByListIdAndUserId(int listId, int userId)
        {
            return this.TodoLists.Include(t => t.Tasks)
                .SingleOrDefault(l => l.Id == listId && l.UserId == userId);
        }

        public List<TodoList> GetTodoListsByUserId(int userId)
        {
            List<TodoList> todoLists = TodoLists.Include(t => t.Tasks)
               .Where(t => t.UserId == userId).ToList();

            return todoLists;
        }

        public TodoTask GetTodoTaskById(int id)
        {
            return TodoTasks.SingleOrDefault(t => t.Id == id);
        }

        public TodoTask GetTodoTaskByIdAndUserId(int taskId, int userId) {
            return TodoTasks.SingleOrDefault(t => t.Id == taskId);
        }

        public void AddUser(User user)
        {
            Users.Add(user);
            SaveChanges();
        }

        public void RemoveUser(User user) {
            Users.Remove(user);
            SaveChanges();
        }

        public void AddTodoList(TodoList todoList)
        {
            TodoLists.Add(todoList);
            SaveChanges();
        }

        public void AddTodoTask(TodoTask todoTask)
        {
            TodoTasks.Add(todoTask);
            SaveChanges();
        }

        public  void RemoveTodoList(TodoList todoList)
        {
            TodoLists.Remove(todoList);
            SaveChanges();
        }

        public async void UpdateTodoList(TodoList todoList)
        {
            TodoLists.Update(todoList);
            await SaveChangesAsync();
        }

        public  void UpdateTodoTask(TodoTask todoTask)
        {
            TodoTasks.Update(todoTask);
             SaveChanges();
        }

        public async void RemoveTodoTask(TodoTask todoTask)
        {
            Remove(todoTask);
            await SaveChangesAsync();
        }

    }
}
