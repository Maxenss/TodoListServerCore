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

        User GetUserById(int id);
        User GetUserByEmail(string email);
        User GetUserByEmailAndPassword(string email, string password);

        TodoList GetTodoListByTitleAndUserId(string title, int userId);
        TodoList GetTodoListByListIdAndUserId(int listId, int userId);
        List<TodoList> GetTodoListsByUserId(int userId);

        TodoTask GetTodoTaskById(int id);

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
