using System.ComponentModel.DataAnnotations;

namespace TasksManager.WebAPI.Dto
{
    public class UpdateStatusDto
    {
        [Required]
        public int Id { get; set; }
    }
}