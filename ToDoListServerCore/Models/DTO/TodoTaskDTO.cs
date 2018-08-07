using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ToDoListServerCore.Models.TodoTask;

namespace ToDoListServerCore.Models.DTO
{
    public class TodoTaskDTO
    {
        public int Id { set; get; }

        public int ToDoListId { set; get; }

        public string Title { set; get; }

        public string Description { set; get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status TaskStatus { set; get; }

        public TodoTaskDTO(TodoTask todoTask)
        {
            Id = todoTask.Id;
            ToDoListId = todoTask.ToDoListId;
            Title = todoTask.Title;
            Description = todoTask.Description;
            TaskStatus = todoTask.TaskStatus;
        }

        public TodoTaskDTO() { }
    }
}
