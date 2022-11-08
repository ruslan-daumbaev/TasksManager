namespace TasksManager.WebAPI.Dto
{
    public record TaskDetailsDto : BaseTaskDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string AddedDate { get; set; }
    }
}