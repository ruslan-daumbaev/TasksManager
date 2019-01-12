using System.Threading.Tasks;
using TasksManager.WebAPI.Models;

namespace TasksManager.WebAPI.Hubs
{
    public interface ITasksHub
    {
        Task Notify(TaskChangedEvent changeEvent);
    }
}