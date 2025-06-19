using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.DTOs;
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
        [HttpGet("{userId}/tasks/simple")]
        public async Task<IActionResult> GetUserTasksSimple(int userId)
        {
            var tasks = await _taskService.GetTasksForUserAsync(userId);

            var result = tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                TaskType = t.TaskType,
                Status = t.Status,
                IsClosed = t.IsClosed,
                ProcurementData = t.ProcurementData == null ? null : new ProcurementDataDto
                {
                    Offer1 = t.ProcurementData.Offer1,
                    Offer2 = t.ProcurementData.Offer2,
                    Receipt = t.ProcurementData.Receipt
                },
                DevelopmentData = t.DevelopmentData == null ? null : new DevelopmentDataDto
                {
                    Specification = t.DevelopmentData.Specification,
                    BranchName = t.DevelopmentData.BranchName,
                    Version = t.DevelopmentData.Version
                }
            });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _taskService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
