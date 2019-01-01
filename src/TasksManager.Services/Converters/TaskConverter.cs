using TasksManager.Data.Entities;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.Services.Converters
{
    public static class TaskConverter
    {
        public static TaskData ConvertToTaskData(this Task task)
        {
            return new TaskData
            {
                Id = task.Id,
                Description = task.Description,
                IsDeleted = task.IsDeleted,
                AddedDate = task.AddedDate,
                Priority = task.Priority,
                TimeToComplete = task.TimeToComplete
            };
        }
    }
}