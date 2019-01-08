using System;
using System.ComponentModel.DataAnnotations;

namespace TasksManager.Web.Models
{
    public abstract class TaskModelBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public byte Priority { get; set; }
    }
}