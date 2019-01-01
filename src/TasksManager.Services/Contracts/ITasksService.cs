using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.Services.Contracts
{
    public interface ITasksService
    {
        Task<IList<TaskData>> GetAllTasksAsync();

        Task<TaskData> GetTask(int id);

        TaskData CreateTask();
    }
}
