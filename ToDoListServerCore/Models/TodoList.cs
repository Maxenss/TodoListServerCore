using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListServerCore.Models
{
    public class TodoList
    {
        [Required]
        public int Id { set; get; }

        [Required]
        public int UserId { set; get; }

        [Required]
        public string Title { set; get; }

        public List<TodoTask> Tasks { set; get; }

        public TodoList() { }

        public TodoList(int userId, string title)
        {
            UserId = userId;
            Title = title;
            Tasks = new List<TodoTask>();
        }
    }
}
