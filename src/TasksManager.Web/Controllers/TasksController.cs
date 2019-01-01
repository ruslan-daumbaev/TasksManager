using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TasksManager.Services.Contracts;
using TasksManager.Services.Exceptions;

namespace TasksManager.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ITasksService tasksService;

        public TasksController(ILogger<TasksController> logger, ITasksService tasksService)
        {
            this.logger = logger;
            this.tasksService = tasksService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tasks = await tasksService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var task = await tasksService.GetTask(id);
                return Ok(task);
            }
            catch (TaskNotFoundException e)
            {
                logger.LogInformation(e, "Couldn't retrieve task");
                return NotFound();
            }
        }
    }
}