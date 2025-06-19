using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.DTOs;
using TaskManagement.API.Models;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskWorkflowService _workflowService;
        private readonly AppDbContext _context;

        public TasksController(TaskWorkflowService workflowService, AppDbContext context)
        {
            _workflowService = workflowService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            if (dto.TaskType != "Procurement" && dto.TaskType != "Development")
                return BadRequest("Unsupported task type");

            var user = await _context.Users.FindAsync(dto.AssignedUserId);
            if (user == null)
                return BadRequest("Assigned user not found");

            var task = new Models.Task
            {
                TaskType = dto.TaskType,
                AssignedUserId = dto.AssignedUserId,
                Status = 1,
                IsClosed = false
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            if (task.TaskType == "Procurement")
                _context.ProcurementData.Add(new ProcurementData { TaskId = task.Id });

            else if (task.TaskType == "Development")
                _context.DevelopmentData.Add(new DevelopmentData { TaskId = task.Id });

            await _context.SaveChangesAsync();
            return Ok(new { task.Id, Message = "Task created" });
        }

        [HttpPut("{taskId}/data/procurement")]
        public async Task<IActionResult> UpdateProcurementData(int taskId, [FromBody] UpdateProcurementDataDto dto)
        {
            var data = await _context.ProcurementData.FirstOrDefaultAsync(p => p.TaskId == taskId);
            if (data == null)
                return NotFound("Procurement data not found");

            if (dto.Offer1 != null) data.Offer1 = dto.Offer1;
            if (dto.Offer2 != null) data.Offer2 = dto.Offer2;
            if (dto.Receipt != null) data.Receipt = dto.Receipt;

            await _context.SaveChangesAsync();
            return Ok("Procurement task data updated");
        }

        [HttpPut("{taskId}/data/development")]
        public async Task<IActionResult> UpdateDevelopmentData(int taskId, [FromBody] UpdateDevelopmentDataDto dto)
        {
            var data = await _context.DevelopmentData.FirstOrDefaultAsync(d => d.TaskId == taskId);
            if (data == null)
                return NotFound("Development data not found");

            if (dto.Specification != null) data.Specification = dto.Specification;
            if (dto.BranchName != null) data.BranchName = dto.BranchName;
            if (dto.Version != null) data.Version = dto.Version;

            await _context.SaveChangesAsync();
            return Ok("Development task data updated");
        }

        [HttpPost("{taskId}/change-status")]
        public async Task<IActionResult> ChangeStatus(int taskId, [FromQuery] int nextStatus, [FromQuery] int newUserId)
        {
            var result = await _workflowService.ChangeStatusAsync(taskId, nextStatus, newUserId);
            return result ? Ok("Status updated") : BadRequest("Invalid transition");
        }
        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.ProcurementData)
                .Include(t => t.DevelopmentData)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound(new { error = "Task not found" });

            var dto = new TaskDto
            {
                Id = task.Id,
                TaskType = task.TaskType,
                IsClosed = task.IsClosed,
                Status = task.Status,
                DevelopmentData = task.DevelopmentData == null ? null : new DevelopmentDataDto
                {
                    Specification = task.DevelopmentData.Specification,
                    BranchName = task.DevelopmentData.BranchName,
                    Version = task.DevelopmentData.Version
                },
                ProcurementData = task.ProcurementData == null ? null : new ProcurementDataDto
                {
                    Offer1 = task.ProcurementData.Offer1,
                    Offer2 = task.ProcurementData.Offer2,
                    Receipt = task.ProcurementData.Receipt
                }
            };

            return Ok(dto);
        }


        [HttpPost("{taskId}/close")]
        public async Task<IActionResult> CloseTask(int taskId)
        {
            var result = await _workflowService.CloseTaskAsync(taskId);
            return result ? Ok("Task closed") : BadRequest("Cannot close task");
        }
    }
}
