using System.Linq;
using System.Threading.Tasks;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using TasksManager.Services.BusinessObjects;
using TasksManager.Services.Contracts;
using TasksManager.WebAPI.Models;

namespace TasksManager.WebAPI.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService tasksService;


        public TasksController(ITasksService tasksService)
        {
            this.tasksService = tasksService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(int page, int pageSize, string statusFilter, string sortField,
            int sortOrder)
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
                    AddedDate = task.AddedDateString
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

            return Ok(new TaskModel(task));
        }



        [HttpPut]
        public async Task<IActionResult> Put(UpdateStatusModel updateModel)
        {
            await tasksService.CompleteTaskAsync(updateModel.Id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await tasksService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}