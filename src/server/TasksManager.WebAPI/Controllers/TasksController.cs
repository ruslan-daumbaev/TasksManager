using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TasksManager.Services.BusinessObjects;
using TasksManager.Services.Interfaces;
using TasksManager.WebAPI.Dto;

namespace TasksManager.WebAPI.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService tasksService;
        private readonly ISecurityService securityService;

        public TasksController(ITasksService tasksService, ISecurityService securityService)
        {
            this.tasksService = tasksService;
            this.securityService = securityService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(int page, int pageSize, string statusFilter, string sortField,
            int sortOrder, CancellationToken token)
        {
            var tasksData = await tasksService.GetAllTasksAsync(page, pageSize, statusFilter, sortField, sortOrder, token);
            return Ok(new
            {
                Tasks = tasksData.Tasks.Select(t => new TaskDto(t)).ToList(),
                tasksData.TotalRecords
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken token)
        {
            var task = await tasksService.GetTaskAsync(id, token);
            return Ok(new TaskDetailsDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status.ToString(),
                Priority = task.Priority,
                AddedDate = task.AddedDateString
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTaskDto model, CancellationToken token)
        {
            var task = await tasksService.CreateTaskAsync(new TaskData
            {
                Name = model.Name,
                Description = securityService.SanitizeText(model.Description),
                Priority = model.Priority,
                TimeToComplete = model.TimeToComplete
            }, token);

            return Ok(new TaskDto(task));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateStatusDto updateModel, CancellationToken token)
        {
            await tasksService.CompleteTaskAsync(updateModel.Id, token);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
        {
            await tasksService.DeleteTaskAsync(id, token);
            return NoContent();
        }
    }
}