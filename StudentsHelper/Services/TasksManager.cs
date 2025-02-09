using SQLite;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;

namespace StudentsHelper.Services
{
    public class TasksManager : ITasksManager
    {
        private SQLiteAsyncConnection database;

        private readonly string dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StudentsHelper-Tasks.sqlite");
        private readonly SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        private async Task<bool> Init()
        {
            if (database is not null) return true;
            try
            {
                database = new SQLiteAsyncConnection(dbPath, flags);
                await database.CreateTableAsync<TaskItem>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<TaskItem>> GetTaskItemsAsync()
        {
            if (await Init())
            {
                return await database.Table<TaskItem>().ToListAsync();
            }
            return [];
        }

        public async Task<TaskItem> GetTaskItemAsync(int id)
        {
            if (await Init())
            {
                return await database.Table<TaskItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
            }
            return new();
        }

        public async Task<int> DeleteTaskItemAsync(TaskItem item)
        {
            if (await Init())
            {
                if (item?.Photos is not null)
                {
                    foreach (var photo in item.Photos)
                    {
                        File.Delete(photo);
                    }
                }
                return await database.DeleteAsync(item);
            }
            return -1;
        }

        public async Task<int> StoreTaskItemAsync(TaskItem item)
        {
            if (await Init())
            {
                if (item.Id != 0)
                {
                    return await database.UpdateAsync(item);
                }
                else
                {
                    return await database.InsertAsync(item);
                }
            }
            return -1;
        }

        public async Task<int> FinishTaskItem(int id)
        {
            if (await Init())
            {
                var item = await database.Table<TaskItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
                if (item is not null)
                {
                    item.IsCompleted = true;
                    return await database.UpdateAsync(item);
                }
            }
            return -1;
        }

        public async Task<List<TaskItem>> GetFinishedTasksAsync()
        {
            if (await Init())
            {
                return await database.Table<TaskItem>().Where(i => i.IsCompleted).ToListAsync();
            }
            return [];
        }

        public async Task<List<TaskItem>> GetPendingTasksAsync()
        {
            if (await Init())
            {
                return await database.Table<TaskItem>().Where(i => !i.IsCompleted).ToListAsync();
            }
            return [];
        }
    }
}