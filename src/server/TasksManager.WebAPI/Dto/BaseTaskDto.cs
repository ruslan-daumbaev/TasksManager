using System.ComponentModel.DataAnnotations;

namespace TasksManager.WebAPI.Dto
{
    public abstract record BaseTaskDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public byte Priority { get; set; }
    }
}