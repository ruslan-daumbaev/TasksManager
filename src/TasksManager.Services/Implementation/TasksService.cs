using System;
using System.Collections.Generic;
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

        public async Task<IList<TaskData>> GetAllTasksAsync()
        {
            try
            {
                var tasks = await dbContext.Tasks.Where(t => t.Status != (int) TaskStatus.Deleted).Select(t => t)
                    .ToListAsync();
                return tasks.Select(t => t.ConvertToTaskData()).ToList();
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
    }
}