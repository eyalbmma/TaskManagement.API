using TaskManagement.API.DTOs;
using TaskManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Models;
using TaskManagement.API.Services;

public class ProcurementDataUpdater : ITaskDataUpdater
{
    private readonly AppDbContext _context;

    public ProcurementDataUpdater(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UpdateAsync(int taskId, object dto)
    {
        var typed = dto as UpdateProcurementDataDto;
        if (typed == null) return false;

        var task = await _context.Tasks
            .Include(t => t.ProcurementData)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task == null) return false;

        if (task.ProcurementData == null)
        {
            task.ProcurementData = new ProcurementData { TaskId = task.Id };
            _context.ProcurementData.Add(task.ProcurementData);
        }

        task.ProcurementData.Offer1 = typed.Offer1;
        task.ProcurementData.Offer2 = typed.Offer2;
        task.ProcurementData.Receipt = typed.Receipt;

        await _context.SaveChangesAsync();
        return true;
    }
}
