using System.Linq;
using System.Threading.Tasks;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TasksManager.Services.BusinessObjects;
using TasksManager.Services.Contracts;
using TasksManager.WebAPI.Hubs;
using TasksManager.WebAPI.Models;

namespace TasksManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private const string SignalRMethod = "Notify";
        private readonly ITasksService tasksService;
        private readonly IHubContext<TasksHub> hubContext;


        public TasksController(ITasksService tasksService, IHubContext<TasksHub> hubContext)
        {
            this.tasksService = tasksService;
            this.hubContext = hubContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(int page, int pageSize, string statusFilter, string sortField, int sortOrder)
        {
            var tasksData = await tasksService.GetAllTasksAsync(page, pageSize, statusFilter, sortField, sortOrder);
            return Ok(new
            {
                Tasks = tasksData.Tasks.Select(t => new TaskModel(t)).ToList(),
                tasksData.TotalRecords
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await tasksService.GetTaskAsync(id);
            return Ok(new
                {
                    task.Id,
                    task.Name,
                    task.Description,
                    Status = task.Status.ToString(),
                    task.Priority,
                    task.AddedDate
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTaskModel model)
        {
            var sanitizer = new HtmlSanitizer();
            var task = await tasksService.CreateTaskAsync(new TaskData
            {
                Name = model.Name,
                Description = sanitizer.Sanitize(model.Description),
                Priority = model.Priority,
                TimeToComplete = model.TimeToComplete
            });

            await hubContext.Clients.All.SendAsync(SignalRMethod, new TaskChangedEvent
            {
                Id = task.Id,
                Change = ChangeType.Added.ToString()
            });

            return Ok(new TaskModel(task));
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateStatusModel updateModel)
        {
            await tasksService.CompleteTaskAsync(updateModel.Id);
            await hubContext.Clients.All.SendAsync(SignalRMethod, new TaskChangedEvent
            {
                Id = updateModel.Id,
                Change = ChangeType.Completed.ToString()
            });
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await tasksService.DeleteTaskAsync(id);
            await hubContext.Clients.All.SendAsync(SignalRMethod, new TaskChangedEvent
            {
                Id = id,
                Change = ChangeType.Deleted.ToString()
            });
            return NoContent();
        }
    }
}