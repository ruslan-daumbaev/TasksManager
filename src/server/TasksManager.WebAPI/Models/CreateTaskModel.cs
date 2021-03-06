﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TasksManager.WebAPI.Models
{
    public class CreateTaskModel : TaskModelBase
    {
        public string Description { get; set; }

        [Required]
        public DateTime TimeToComplete { get; set; }
    }
}