using TaskEntity = TaskManagement.API.Models.Task;

namespace TaskManagement.API.Rules
{
    public class ProcurementRules : ITaskRules
    {
        public int FinalStatus => 3;

        public bool CanMoveForward(TaskEntity task)
        {
            if (task.ProcurementData == null) return false;

            return task.Status switch
            {
                1 => !string.IsNullOrWhiteSpace(task.ProcurementData.Offer1) &&
                     !string.IsNullOrWhiteSpace(task.ProcurementData.Offer2),
                2 => !string.IsNullOrWhiteSpace(task.ProcurementData.Receipt),
                _ => false
            };
        }

        public bool CanMoveBackward(TaskEntity task)
        {
            return task.Status > 1;
        }

        public bool CanClose(TaskEntity task)
        {
            return task.Status == FinalStatus;
        }
    }
}
