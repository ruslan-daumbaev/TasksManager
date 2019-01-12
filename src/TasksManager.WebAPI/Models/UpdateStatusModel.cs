using System.ComponentModel.DataAnnotations;

namespace TasksManager.WebAPI.Models
{
    public class UpdateStatusModel
    {
        [Required]
        public int Id { get; set; }
    }
}