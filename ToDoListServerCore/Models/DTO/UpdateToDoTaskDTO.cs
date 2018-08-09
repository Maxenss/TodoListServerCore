using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListServerCore.Models.DTO
{
    public class UpdateTodoTaskDTO
    {
        public int TaskId { set; get; }

        public int ToDoListId { set; get; }

        public string Title { set; get; }

        public string Description { set; get; }
    }
}
