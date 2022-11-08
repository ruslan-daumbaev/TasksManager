using TasksManager.Services.BusinessObjects;

namespace TasksManager.WebAPI.Dto
{
    public record TaskDto : BaseTaskDto
    {
        public TaskDto(TaskData data)
        {
            Id = data.Id;
            Name = data.Name;
            Priority = data.Priority;
            AddedDate = data.AddedDateString;
            Status = data.Status.ToString();
            TimeToComplete = data.TimeToCompleteString;
        }

        public int Id { get; set; }

        public string AddedDate { get; set; }

        public string Status { get; set; }

        public string TimeToComplete { get; set; }
    }
}