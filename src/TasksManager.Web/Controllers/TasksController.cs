using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TasksManager.Services.BusinessObjects;
using TasksManager.Services.Contracts;
using TasksManager.Web.Models;

namespace TasksManager.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService tasksService;

        public TasksController(ITasksService tasksService)
        {
            this.tasksService = tasksService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var tasks = await tasksService.GetAllTasksAsync();
            return Ok(tasks.Select(t => new TaskModel(t)).ToArray());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await tasksService.GetTaskAsync(id);
            return Ok(new TaskModel(task));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskModel model)
        {
            var task = await tasksService.CreateTaskAsync(new TaskData
            {
                Name = model.Name,
                Description = model.Description,
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