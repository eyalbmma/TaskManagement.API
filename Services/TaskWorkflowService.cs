using TaskManagement.API.Data;
using TaskManagement.API.Models;
using TaskManagement.API.Rules;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.API.Services
{
    public class TaskWorkflowService
    {
        private readonly AppDbContext _context;

        public TaskWorkflowService(AppDbContext context)
        {
            _context = context;
        }

        private ITaskRules GetRulesFor(Models.Task task)
        {
            return task.TaskType switch
            {
                "Procurement" => new ProcurementRules(),
                "Development" => new DevelopmentRules(),
                _ => throw new Exception("Unknown task type")
            };
        }

        public async Task<bool> ChangeStatusAsync(int taskId, int nextStatus, int newUserId)
        {
            var task = await _context.Tasks
                .Include(t => t.ProcurementData)
                .Include(t => t.DevelopmentData)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null || task.IsClosed)
                return false;

            var rules = GetRulesFor(task);

            if (nextStatus == task.Status + 1)
            {
                if (!rules.CanMoveForward(task))
                    return false;
            }
            else if (nextStatus < task.Status)
            {
                if (!rules.CanMoveBackward(task))
                    return false;
            }
            else
            {
                return false; // מעבר לא חוקי
            }

            task.Status = nextStatus;
            task.AssignedUserId = newUserId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseTaskAsync(int taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.ProcurementData)
                .Include(t => t.DevelopmentData)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null || task.IsClosed)
                return false;

            var rules = GetRulesFor(task);
            if (!rules.CanClose(task))
                return false;

            task.IsClosed = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
