using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListServerCore.Models.DTO
{
    public class UserDTO
    {
        public string Token { get; set; }

        public DateTime TokenExpires { get; set; }

        public DateTime ResponseTime { get; set; }

        public User User { get; set; }

        public UserDTO(string token, DateTime tokenExpires, DateTime responseTime, User user)
        {
            Token = token;
            TokenExpires = tokenExpires;
            ResponseTime = responseTime;
            User = user;
        }
    }
}
