using System.Threading;
using System.Threading.Tasks;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.Services.Interfaces
{
    public interface ITasksService
    {
        Task<TasksPagedData> GetAllTasksAsync(int? startFrom, int? pageSize, string status, string sortField, int? sortOrder, CancellationToken token);

        Task<TaskData> GetTaskAsync(int id, CancellationToken token);

        Task<TaskData> CreateTaskAsync(TaskData newTask, CancellationToken token);

        Task CompleteTaskAsync(int id, CancellationToken token);

        Task DeleteTaskAsync(int id, CancellationToken token);
    }
}