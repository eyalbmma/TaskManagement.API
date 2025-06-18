using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserTaskService _taskService;

        public UsersController(UserTaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{userId}/tasks")]
        public async Task<IActionResult> GetUserTasks(int userId)
        {
            var tasks = await _taskService.GetTasksForUserAsync(userId);
            return Ok(tasks);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _taskService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
