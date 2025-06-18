using TaskManagement.API.Models;

namespace TaskManagement.API.Rules
{
    public interface ITaskRules
    {
        int FinalStatus { get; }
        bool CanMoveForward(Models.Task task);
        bool CanMoveBackward(Models.Task task);
        bool CanClose(Models.Task task);
    }
}
