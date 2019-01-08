using System;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.Web.Models
{
    public class TaskModel : TaskModelBase
    {
        public TaskModel()
        {
        }

        public TaskModel(TaskData data)
        {
            Id = data.Id;
            Name = data.Name;
            AddedDate = data.AddedDate;
            Status = data.Status.ToString();
            TimeToComplete = data.TimeToComplete.ToString("u");
        }

        public int Id { get; set; }

        public DateTime AddedDate { get; set; }

        public string Status { get; set; }

        public string TimeToComplete { get; set; }
    }
}