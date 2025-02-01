using StudentsHelper.Models;

namespace StudentsHelper.Interfaces
{
    public interface ITasksManager
    {
        Task<List<TaskItem>> GetTaskItemsAsync();
        Task<TaskItem> GetTaskItemAsync(int id);
        Task<int> StoreTaskItemAsync(TaskItem item);
        Task<int> DeleteTaskItemAsync(TaskItem item);
        Task<List<TaskItem>> GetFinishedTasksAsync();
        Task<List<TaskItem>> GetPendingTasksAsync();
        Task<int> FinishTaskItem(int id);
    }
}