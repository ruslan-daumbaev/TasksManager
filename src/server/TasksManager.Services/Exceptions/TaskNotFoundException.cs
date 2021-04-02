using System;

namespace TasksManager.Services.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int taskId) : base($"Task with Id={taskId} was not found")
        {
        }
    }
}