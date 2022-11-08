using System.Threading.Tasks;

namespace TasksManager.WebAPI.Hubs
{
    public interface ITasksHub
    {
        Task Notify(TaskChangedEvent changeEvent);
    }
}