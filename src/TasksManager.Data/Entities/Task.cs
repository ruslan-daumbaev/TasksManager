using System;
using System.ComponentModel.DataAnnotations;

namespace TasksManager.Data.Entities
{
    public class Task
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public byte Priority { get; set; }

        public DateTime TimeToComplete { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime ChangeDate { get; set; }

        public int Status { get; set; }

    }
}
