using System;
using Prism.Mvvm;
using ToDoListDemo.Abstractions.Tasks;

namespace ToDoListDemo.Models
{
    public class TaskItem : BindableBase
    {
        public ITaskModel Task { get; }

        public string Name
        {
            get { return Task.Name; }
            set
            {
                Task.Name = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return Task.Description; }
            set
            {
                Task.Description = value;
                RaisePropertyChanged();
            }
        }

        public TaskStatus Status
        {
            get { return Task.Status; }
            set
            {
                Task.Status = value;
                RaisePropertyChanged();
            }
        }

        public TaskItem(ITaskModel task)
        {
            Task = task;
        }

        public void Update(ITaskModel task)
        {
            Name = task.Name;
            Description = task.Description;
            Status = task.Status;
        }
    }
}
