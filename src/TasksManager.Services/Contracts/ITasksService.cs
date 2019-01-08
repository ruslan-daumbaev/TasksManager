using System.Threading.Tasks;
using TasksManager.Services.BusinessObjects;
using TaskStatus = TasksManager.Services.BusinessObjects.TaskStatus;

namespace TasksManager.Services.Contracts
{
    public interface ITasksService
    {
        Task<TasksPagedData> GetAllTasksAsync(int startFrom, int pageSize, string status, string sortField, int sortOrder);

        Task<TaskData> GetTaskAsync(int id);

        Task<TaskData> CreateTaskAsync(TaskData newTask);

        Task CompleteTaskAsync(int id);

        Task DeleteTaskAsync(int id);
    }
}