using TaskEntity = TaskManagement.API.Models.Task;

namespace TaskManagement.API.Rules
{
    public class DevelopmentRules : ITaskRules
    {
        public int FinalStatus => 4;

        public bool CanMoveForward(TaskEntity task)
        {
            if (task.DevelopmentData == null) return false;

            return task.Status switch
            {
                1 => !string.IsNullOrWhiteSpace(task.DevelopmentData.Specification),
                2 => !string.IsNullOrWhiteSpace(task.DevelopmentData.BranchName),
                3 => !string.IsNullOrWhiteSpace(task.DevelopmentData.Version),
                _ => false
            };
        }

        public bool CanMoveBackward(TaskEntity task)
        {
            return task.Status > 1;
        }

        public bool CanClose(TaskEntity task)
        {
            if (task.DevelopmentData == null) return false;

            return task.Status == 4 &&
                   !string.IsNullOrWhiteSpace(task.DevelopmentData.Specification) &&
                   !string.IsNullOrWhiteSpace(task.DevelopmentData.BranchName) &&
                   !string.IsNullOrWhiteSpace(task.DevelopmentData.Version);
        }

    }
}
