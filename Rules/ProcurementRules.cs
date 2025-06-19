using TaskEntity = TaskManagement.API.Models.Task;

namespace TaskManagement.API.Rules
{
    public class ProcurementRules : ITaskRules
    {
        public int FinalStatus => 4; // ← תיקון כאן

        public bool CanMoveForward(TaskEntity task)
        {
            if (task.ProcurementData == null) return false;

            return task.Status switch
            {
                1 => !string.IsNullOrWhiteSpace(task.ProcurementData.Offer1) &&
                     !string.IsNullOrWhiteSpace(task.ProcurementData.Offer2),
                2 => !string.IsNullOrWhiteSpace(task.ProcurementData.Receipt),
                3 => true, // מ־3 ל־4
                _ => false
            };
        }

        public bool CanMoveBackward(TaskEntity task)
        {
            return task.Status > 1;
        }

        public bool CanClose(TaskEntity task)
        {
            if (task.ProcurementData == null) return false;

            return task.Status == 4 &&
                   !string.IsNullOrWhiteSpace(task.ProcurementData.Offer1) &&
                   !string.IsNullOrWhiteSpace(task.ProcurementData.Offer2) &&
                   !string.IsNullOrWhiteSpace(task.ProcurementData.Receipt);
        }

    }

}
