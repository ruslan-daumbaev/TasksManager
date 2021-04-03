using TasksManager.Data.Entities;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.Services.Converters
{
    public static class TaskConverter
    {
        public static TaskData ConvertToTaskData(this TodoTask task)
        {
            return new TaskData
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                AddedDate = task.AddedDate,
                Priority = task.Priority,
                TimeToComplete = task.TimeToComplete,
                Status = (TaskStatus) task.Status
            };
        }

        public static TodoTask ConvertToTask(this TaskData taskData)
        {
            return new TodoTask
            {
                Id = taskData.Id,
                Name = taskData.Name,
                Description = taskData.Description,
                Priority = taskData.Priority,
                AddedDate = taskData.AddedDate,
                TimeToComplete = taskData.TimeToComplete,
                Status = (int) taskData.Status
            };
        }
    }
}