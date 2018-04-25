using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
    public partial class User
    {
        public User()
        {
            Todo = new HashSet<Todo>();
        }

        public uint Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset RegisterTime { get; set; }

        public ICollection<Todo> Todo { get; set; }
    }
}
