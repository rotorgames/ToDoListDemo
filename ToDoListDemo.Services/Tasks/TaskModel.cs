using System;
using SQLite;
using ToDoListDemo.Abstractions.Tasks;

namespace ToDoListDemo.Services.Tasks
{
    [Table("tasks")]
    public class TaskModel : ITaskModel
    {
        [Column("id"), PrimaryKey]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("creating_date")]
        public DateTime CreatingDate { get; set; }

        [Column("status")]
        public TaskStatus Status { get; set; }

        public TaskModel()
        {
            Id = Guid.NewGuid();
        }

        internal TaskModel(Guid id)
        {
            Id = id;
        }
    }
}
