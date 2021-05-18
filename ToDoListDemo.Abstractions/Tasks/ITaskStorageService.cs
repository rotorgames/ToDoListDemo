using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoListDemo.Abstractions.Tasks
{
    public interface ITaskStorageService
    {
        Task SetTaskAsync(ITaskModel model);

        Task<IList<ITaskModel>> GetTasksAsync(int offset, int length);

        Task<ITaskModel> GetTaskAsync(Guid id);

        Task DeleteTaskAsync(Guid id);

        Task<int> GetTasksQuantityAsync();
    }
}
