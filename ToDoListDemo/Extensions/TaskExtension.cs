using System;
using ToDoListDemo.Abstractions.Tasks;
using ToDoListDemo.LocalizableResources;

namespace ToDoListDemo.Extensions
{
    public static class TaskExtension
    {
        public static string GetLocalizableName(this TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Open:
                    return AppResources.TaskStatusOpen;
                case TaskStatus.InProcess:
                    return AppResources.TaskStatusInProcess;
                case TaskStatus.Finished:
                    return AppResources.TaskStatusFinished;

                default:
                    throw new InvalidOperationException($"{status} is not supported");
            }
        }
    }
}
