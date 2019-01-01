﻿using System;

namespace TasksManager.Data.Entities
{
    public class Task
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public byte Priority { get; set; }

        public DateTime TimeToComplete { get; set; }

        public DateTime AddedDate { get; set; }

        public bool IsDeleted { get; set; }

    }
}
