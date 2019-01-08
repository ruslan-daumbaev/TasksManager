using System;


namespace TasksManager.Services.BusinessObjects
{
    public class TaskData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte Priority { get; set; }

        public DateTime TimeToComplete { get; set; }

        public DateTime AddedDate { get; set; }

        public TaskStatus Status { get; set; }
    }
}