using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TasksManager.WebAPI.Models;

namespace TasksManager.WebAPI.Hubs
{
    public class TasksHub : Hub<ITasksHub>
    {
        public async Task Notify(TaskChangedEvent changeEvent)
        {
            await Clients.Others.Notify(changeEvent);
        }
    }
}
