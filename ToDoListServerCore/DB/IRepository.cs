using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListServerCore.Models;

namespace ToDoListServerCore.DB
{
    public interface IRepository
    {
        IEnumerable<User> GetUsers();
        IEnumerable<TodoTask> GetToDoTasks();
        IEnumerable<TodoList> GetToDoLists();

        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByEmailAndPassword(string email, string password);

        Task<TodoList> GetTodoListByTitleAndUserId(string title, int userId);
        Task<TodoList> GetTodoListByListIdAndUserId(int listId, int userId);
        List<TodoList> GetTodoListsByUserId(int userId);

        Task<TodoTask> GetTodoTaskById(int id);
        Task<TodoTask> GetTodoTaskByIdAndUserId(int taskId, int userId);

        void AddUser(User user);
        void RemoveUser(User user);
        void AddTodoList(TodoList todoList);
        void AddTodoTask(TodoTask todoTask);
        void RemoveTodoList(TodoList todoList);
        void RemoveTodoTask(TodoTask todoTask);
        void UpdateTodoList(TodoList todoList);
        void UpdateTodoTask(TodoTask todoTask);
    }
}
