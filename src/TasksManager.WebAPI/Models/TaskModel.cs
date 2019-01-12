using System;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.WebAPI.Models
{
    public class TaskModel : TaskModelBase
    {
        public TaskModel(TaskData data)
        {
            Id = data.Id;
            Name = data.Name;
            Priority = data.Priority;
            AddedDate = data.AddedDateString;
            Status = data.Status.ToString();
            TimeToComplete = data.TimeToCompleteString;
        }

        public int Id { get; set; }

        public string AddedDate { get; set; }

        public string Status { get; set; }

        public string TimeToComplete { get; set; }
    }
}