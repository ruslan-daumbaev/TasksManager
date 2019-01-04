using System;
using System.ComponentModel.DataAnnotations;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.Web.Models
{
    public class TaskModel
    {
        public TaskModel()
        {
        }

        public TaskModel(TaskData data)
        {
            Id = data.Id;
            Name = data.Name;
            Description = data.Description;
            Priority = data.Priority;
            TimeToComplete = data.TimeToComplete;
            AddedDate = data.AddedDate;
            Status = data.Status.ToString();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public byte Priority { get; set; }

        [Required]
        public DateTime TimeToComplete { get; set; }

        public DateTime AddedDate { get; set; }

        public string Status { get; set; }
    }
}