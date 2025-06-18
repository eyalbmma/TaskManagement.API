using TaskManagement.API.DTOs;
using TaskManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Models;
using TaskManagement.API.Services;

public class DevelopmentDataUpdater : ITaskDataUpdater
{
    private readonly AppDbContext _context;

    public DevelopmentDataUpdater(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UpdateAsync(int taskId, object dto)
    {
        var typed = dto as UpdateDevelopmentDataDto;
        if (typed == null) return false;

        var task = await _context.Tasks
            .Include(t => t.DevelopmentData)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task == null) return false;

        if (task.DevelopmentData == null)
        {
            task.DevelopmentData = new DevelopmentData { TaskId = task.Id };
            _context.DevelopmentData.Add(task.DevelopmentData);
        }

        task.DevelopmentData.Specification = typed.Specification;
        task.DevelopmentData.BranchName = typed.BranchName;
        task.DevelopmentData.Version = typed.Version;

        await _context.SaveChangesAsync();
        return true;
    }
}
