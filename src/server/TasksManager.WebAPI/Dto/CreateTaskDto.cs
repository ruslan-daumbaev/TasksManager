using System;
using System.ComponentModel.DataAnnotations;

namespace TasksManager.WebAPI.Dto
{
    public record CreateTaskDto : BaseTaskDto
    {
        public string Description { get; set; }

        [Required]
        public DateTime TimeToComplete { get; set; }
    }
}