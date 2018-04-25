using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
    public partial class Todo
    {
        public uint Id { get; set; }
        public uint UserId { get; set; }
        public string Content { get; set; }
        public bool IsCompleted { get; set; }
        public DateTimeOffset CompletedTime { get; set; }

        public User User { get; set; }
    }
}
