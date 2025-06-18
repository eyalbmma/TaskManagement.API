using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.DTOs;
using TaskManagement.API.Models;

public class MvcTaskService
{
    private readonly AppDbContext _context;

    public MvcTaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ChangeTaskStatusAsync(int taskId, int nextStatus, int newUserId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null) return false;

        task.Status = nextStatus;
        task.AssignedUserId = newUserId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CloseTaskAsync(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null || task.IsClosed) return false;

        task.IsClosed = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CreateTaskAsync(string type, int userId)
    {
        var task = new TaskManagement.API.Models.Task
        {
            TaskType = type,
            AssignedUserId = userId,
            Status = 1,
            IsClosed = false
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync(); // שים לב! חייב לרוץ לפני השורות הבאות כדי ש־task.Id יהיה זמין

        if (type == "Procurement")
        {
            task.ProcurementData = new ProcurementData { TaskId = task.Id };
            _context.ProcurementData.Add(task.ProcurementData);
        }
        else if (type == "Development")
        {
            task.DevelopmentData = new DevelopmentData { TaskId = task.Id };
            _context.DevelopmentData.Add(task.DevelopmentData);
        }

        await _context.SaveChangesAsync();
        return true;
    }




}
