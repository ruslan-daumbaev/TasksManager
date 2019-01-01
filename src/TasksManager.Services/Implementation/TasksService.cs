using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data.DataContext;
using TasksManager.Services.BusinessObjects;
using TasksManager.Services.Contracts;
using TasksManager.Services.Converters;
using TasksManager.Services.Exceptions;

namespace TasksManager.Services.Implementation
{
    public class TasksService : ITasksService
    {
        private readonly TasksManagerDbContext dbContext;

        public TasksService(TasksManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IList<TaskData>> GetAllTasksAsync()
        {
            var tasks = await dbContext.Tasks.Where(t => !t.IsDeleted).Select(t => t).ToListAsync();

            return tasks.Select(t => t.ConvertToTaskData()).ToList();
        }

        public async Task<TaskData> GetTask(int id)
        {
            var task = await dbContext.Tasks.FirstOrDefaultAsync(t => !t.IsDeleted && t.Id == id);
            if (task == null)
            {
                throw new TaskNotFoundException(id);
            }

            return task.ConvertToTaskData();
        }

        public TaskData CreateTask()
        {
            throw new NotImplementedException();
        }
    }
}