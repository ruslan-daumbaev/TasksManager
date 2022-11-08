using System.ComponentModel.DataAnnotations;

namespace TasksManager.WebAPI.Dto
{
    public class UpdateTaskDto
    {
        [Required]
        public int Id { get; set; }
    }
}