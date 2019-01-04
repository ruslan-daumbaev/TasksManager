using System.Collections.Generic;
using System.Threading.Tasks;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.Services.Contracts
{
    public interface ITasksService
    {
        Task<IList<TaskData>> GetAllTasksAsync();

        Task<TaskData> GetTaskAsync(int id);

        Task<TaskData> CreateTaskAsync(TaskData newTask);

        Task CompleteTaskAsync(int id);

        Task DeleteTaskAsync(int id);
    }
}
