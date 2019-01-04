using System;

namespace TasksManager.Services.Exceptions
{
    public class TaskProcessingException : Exception
    {

        public TaskProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TaskProcessingException(Exception innerException) : base("Couldn't retrieve/update task", innerException)
        {
        }
    }
}