using System;

namespace TasksManager.Data.Entities
{
    public class Task
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

        public DateTime TimeToComplete { get; set; }

        public DateTime AddedDate { get; set; }

    }
}
