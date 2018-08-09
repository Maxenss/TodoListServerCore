using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDoListServerCore.Models.DTO;

namespace ToDoListServerCore.Models
{
    public class TodoTask
    {
        public enum Status
        {
            AWAIT = 0, DONE = 1, CANCELED = 2
        }

        [Key]
        public int Id { set; get; }

        [Required]
        public int ToDoListId { set; get; }

        [Required]
        public string Title { set; get; }

        [Required]
        public string Description { set; get; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Status TaskStatus { set; get; }

        public TodoTask() { }

        public TodoTask(int toDoListId, string title, string description)
        {
            ToDoListId = toDoListId;
            Title = title;
            Description = description;
        }

        public TodoTask(CreateTodoTaskDTO createToDoTaskDTO) {
            ToDoListId = createToDoTaskDTO.ToDoListId;
            Title = createToDoTaskDTO.Title;
            Description = createToDoTaskDTO.Description;
        }
    }
}
