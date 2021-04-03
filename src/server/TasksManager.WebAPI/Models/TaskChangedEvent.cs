namespace TasksManager.WebAPI.Models
{
    public class TaskChangedEvent
    {
        public int Id { get; set; }

        public string Change { get; set; }
    }
}