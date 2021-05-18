using System;

namespace ToDoListDemo.Abstractions.Tasks
{
    public interface ITaskModel
    {
        public Guid Id { get; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatingDate { get; set; }

        public TaskStatus Status { get; set; }
    }
}
