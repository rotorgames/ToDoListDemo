using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using ToDoListDemo.Abstractions.Tasks;

namespace ToDoListDemo.Services.Tasks
{
    public class TaskStorageService : ITaskStorageService
    {
        readonly SQLiteAsyncConnection _db;

        public TaskStorageService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);

            _db.CreateTableAsync<TaskModel>().Wait();
        }

        public async Task SetTaskAsync(ITaskModel model)
        {
            await _db.InsertOrReplaceAsync(model);
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            await _db
                .Table<TaskModel>()
                .DeleteAsync(t => t.Id == id);
        }

        public async Task<ITaskModel> GetTaskAsync(Guid id)
        {
            return await _db
                .Table<TaskModel>()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IList<ITaskModel>> GetTasksAsync(int offset, int length)
        {
            var result = await _db
                .Table<TaskModel>()
                .OrderByDescending(t => t.CreatingDate)
                .Skip(offset)
                .Take(length)
                .ToListAsync();

            return result.Cast<ITaskModel>().ToList();
        }

        public async Task<int> GetTasksQuantityAsync()
        {
            return await _db.Table<TaskModel>().CountAsync();
        }
    }
}
