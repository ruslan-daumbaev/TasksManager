using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TasksManager.Data.DataContext;
using TasksManager.Data.Entities;
using TasksManager.Services.BusinessObjects;
using TasksManager.Services.Converters;
using TasksManager.Services.Exceptions;
using TasksManager.Services.Extensions;
using TasksManager.Services.Interfaces;
using TaskStatus = TasksManager.Services.BusinessObjects.TaskStatus;

namespace TasksManager.Services
{
    public class TasksService : ITasksService
    {
        private const int AscOrderValue = 1;
        private const int DefaultPageSize = 100;

        private readonly TasksDbContext dbContext;
        private readonly ILogger logger;

        public TasksService(ILogger<TasksService> logger, TasksDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<TasksPagedData> GetAllTasksAsync(int? startFrom, int? pageSize, string status, string sortField,
            int? sortOrder, CancellationToken token)
        {
            try
            {
                var query = dbContext.Set<TodoTask>().AsQueryable().FilterByStatus(status);
                var totalRecords = await query.CountAsync(token);
                var start = startFrom ?? 0;
                var size = pageSize.HasValue && pageSize.Value > 0 ? pageSize.Value : DefaultPageSize;
                var tasks = await query.OrderByField(sortField, sortOrder == AscOrderValue).Skip(start)
                    .Take(size).AsNoTracking().ToListAsync(token);

                return new TasksPagedData
                {
                    Tasks = tasks.Select(t => t.ConvertToTaskData()).ToArray(),
                    TotalRecords = totalRecords
                };
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException(ex);
            }
        }


        public async Task<TaskData> GetTaskAsync(int id, CancellationToken token) => (await GetTask(id, token)).ConvertToTaskData();

        public async Task<TaskData> CreateTaskAsync(TaskData newTask, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(nameof(newTask));

            logger.LogDebug("Start creation of new task");
            var taskRecord = newTask.ConvertToTask();
            taskRecord.AddedDate = DateTime.UtcNow;
            taskRecord.Status = (int)TaskStatus.Active;
            taskRecord.ChangeDate = DateTime.UtcNow;
            dbContext.Set<TodoTask>().Add(taskRecord);
            try
            {
                await dbContext.SaveChangesAsync(token);
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException("Couldn't create task", ex);
            }

            logger.LogInformation($"New task with id={taskRecord.Id} has been created");
            return taskRecord.ConvertToTaskData();
        }

        public async Task CompleteTaskAsync(int id, CancellationToken token)
        {
            logger.LogDebug($"Start completion for task with Id={id}");

            var task = await GetTask(id, token);
            if (task.Status != (int)TaskStatus.Active)
            {
                return;
            }

            try
            {
                task.Status = (int)TaskStatus.Completed;
                task.ChangeDate = DateTime.Now;
                dbContext.Update(task);
                await dbContext.SaveChangesAsync(token);
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException("Couldn't update task", ex);
            }

            logger.LogDebug($"Task with Id={id} has been successfully marked as completed");
        }

        public async Task DeleteTaskAsync(int id, CancellationToken token)
        {
            logger.LogDebug($"Start deletion for task with Id={id}");
            var task = await dbContext.Set<TodoTask>().FindAsync(new object[] { id }, cancellationToken: token);
            if (task == null || task.Status != (int)TaskStatus.Completed)
            {
                return;
            }

            try
            {
                task.Status = (int)TaskStatus.Deleted;
                task.ChangeDate = DateTime.Now;
                dbContext.Update(task);
                await dbContext.SaveChangesAsync(token);
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException("Couldn't update task", ex);
            }

            logger.LogDebug($"Task with Id={id} has been successfully marked as deleted");
        }


        private async Task<TodoTask> GetTask(int id, CancellationToken token)
        {
            TodoTask task;
            try
            {
                task = await dbContext.Set<TodoTask>().FindAsync(new object[] { id }, cancellationToken: token);
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException("Couldn't retrieve task", ex);
            }

            if (task == null || task.Status == (int)TaskStatus.Deleted)
            {
                throw new TaskNotFoundException(id);
            }

            return task;
        }
    }
}