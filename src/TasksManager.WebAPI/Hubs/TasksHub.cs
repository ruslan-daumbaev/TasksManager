using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TasksManager.WebAPI.Models;

namespace TasksManager.WebAPI.Hubs
{
    public class TasksHub : Hub
    {
        public async Task Send(TaskChangedEvent changeEvent)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", changeEvent);
        }
    }
}
