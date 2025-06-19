using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.Models;

namespace TaskManagement.API.Services
{
    public class UserTaskService
    {
        private readonly AppDbContext _context;

        public UserTaskService(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]


        public async Task<List<Models.Task>> GetAllTasksAsync()
        {
            var tasks = await _context.Tasks
                .Include(t => t.ProcurementData)
                .Include(t => t.DevelopmentData)
                .ToListAsync();

            return tasks;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
        public async Task<List<Models.Task>> GetTasksForUserAsync(int userId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();

            var taskIds = tasks.Select(t => t.Id).ToList();

            var procurementData = await _context.ProcurementData
                .Where(d => taskIds.Contains(d.TaskId))
                .ToListAsync();

            var developmentData = await _context.DevelopmentData
                .Where(d => taskIds.Contains(d.TaskId))
                .ToListAsync();

            // הצמדת המידע לכל משימה לפי סוגה
            foreach (var task in tasks)
            {
                if (task.TaskType == "Procurement")
                {
                    task.ProcurementData = procurementData.FirstOrDefault(d => d.TaskId == task.Id);
                }
                else if (task.TaskType == "Development")
                {
                    task.DevelopmentData = developmentData.FirstOrDefault(d => d.TaskId == task.Id);
                }
            }

            return tasks;
        }
    }
}
