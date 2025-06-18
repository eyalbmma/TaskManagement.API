namespace TaskManagement.API.Services
{
    public interface ITaskDataUpdater
    {
        Task<bool> UpdateAsync(int taskId, object dto);
    }
}
