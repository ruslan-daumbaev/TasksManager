﻿using System.ComponentModel.DataAnnotations;

namespace TasksManager.Web.Models
{
    public class UpdateStatusModel
    {
        [Required]
        public int Id { get; set; }

        [EnumDataType(typeof(TaskStatus))]
        public TaskStatus Status { get; set; }
    }
}