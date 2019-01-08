using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TasksManager.Data.DataContext;
using TasksManager.Services.BusinessObjects;
using TasksManager.Services.Contracts;
using TasksManager.Services.Converters;
using TasksManager.Services.Exceptions;
using TaskStatus = TasksManager.Services.BusinessObjects.TaskStatus;

namespace TasksManager.Services.Implementation
{
    public class TasksService : ITasksService
    {
        private readonly TasksManagerDbContext dbContext;
        private readonly ILogger logger;


        public TasksService(ILogger<TasksService> logger, TasksManagerDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<TasksPagedData> GetAllTasksAsync(int startFrom, int pageSize, string status, string sortField,
            int sortOrder)
        {
            try
            {
                var query = dbContext.Tasks.Where(t => t.Status != (int) TaskStatus.Deleted);
                var statusFilter = GetStatusFilter(status);
                if (statusFilter.HasValue)
                {
                    query = query.Where(r => r.Status == statusFilter.Value);
                }

                var totalRecords = await query.CountAsync();
                if (!string.IsNullOrWhiteSpace(sortField))
                {
                    query = ApplySorting(sortField, sortOrder, query);
                }

                var tasks = await query.Skip(startFrom).Take(pageSize)
                    .ToListAsync();

                return new TasksPagedData
                {
                    Tasks = tasks.Select(t => t.ConvertToTaskData()).ToList(),
                    TotalRecords = totalRecords
                };
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException(ex);
            }
        }


        public async Task<TaskData> GetTaskAsync(int id)
        {
            return (await GetTask(id)).ConvertToTaskData();
        }

        public async Task<TaskData> CreateTaskAsync(TaskData newTask)
        {
            if (newTask == null)
            {
                throw new ArgumentNullException(nameof(newTask));
            }

            logger.LogDebug("Start creation of new task");
            var taskRecord = newTask.ConvertToTask();
            taskRecord.AddedDate = DateTime.Now;
            taskRecord.Status = (int) TaskStatus.Active;
            taskRecord.ChangeDate = DateTime.Now;
            dbContext.Tasks.Add(taskRecord);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException("Couldn't create task", ex);
            }

            logger.LogInformation($"New task with id={taskRecord.Id} has been created");
            return taskRecord.ConvertToTaskData();
        }

        public async Task CompleteTaskAsync(int id)
        {
            logger.LogDebug($"Start completion for task with Id={id}");

            var task = await GetTask(id);
            if (task.Status != (int) TaskStatus.Active)
            {
                return;
            }

            try
            {
                task.Status = (int) TaskStatus.Completed;
                task.ChangeDate = DateTime.Now;
                dbContext.Update(task);
                await dbContext.SaveChangesAsync();
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException("Couldn't update task", ex);
            }

            logger.LogDebug($"Task with Id={id} has been successfully marked as completed");
        }

        public async Task DeleteTaskAsync(int id)
        {
            logger.LogDebug($"Start deletion for task with Id={id}");
            var task = await GetTask(id);
            if (task.Status != (int) TaskStatus.Completed)
            {
                return;
            }

            try
            {
                task.Status = (int) TaskStatus.Deleted;
                task.ChangeDate = DateTime.Now;
                dbContext.Update(task);
                await dbContext.SaveChangesAsync();
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException("Couldn't update task", ex);
            }

            logger.LogDebug($"Task with Id={id} has been successfully marked as deleted");
        }

        private static IQueryable<Data.Entities.Task> ApplySorting(string sortField, int sortOrder,
            IQueryable<Data.Entities.Task> query)
        {
            switch (sortField)
            {
                case "name":
                    return sortOrder == 1 ? query.OrderBy(r => r.Name) : query.OrderByDescending(r => r.Name);
                case "priority":
                    return sortOrder == 1
                        ? query.OrderBy(r => r.Priority)
                        : query.OrderByDescending(r => r.Priority);
                default:
                    return query.OrderBy(r => r.TimeToComplete);
            }
        }

        private async Task<Data.Entities.Task> GetTask(int id)
        {
            Data.Entities.Task task;
            try
            {
                task = await dbContext.Tasks.FindAsync(id);
            }
            catch (SqlException ex)
            {
                throw new TaskProcessingException("Couldn't retrieve task", ex);
            }

            if (task == null || task.Status == (int) TaskStatus.Deleted)
            {
                throw new TaskNotFoundException(id);
            }

            return task;
        }


        private static int? GetStatusFilter(string status)
        {
            if (status == null) return null;
            int? statusFilter = null;
            if (Enum.TryParse(status, out TaskStatus parsedStatus))
            {
                statusFilter = (int) parsedStatus;
            }

            return statusFilter;
        }
    }
}