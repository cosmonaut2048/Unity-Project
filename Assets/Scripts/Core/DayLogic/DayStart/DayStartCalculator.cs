using Core.DayLogic.TickCalculation;
using Core.TaskLogic;
using Runtime;
using Random = UnityEngine.Random;

namespace Core.DayLogic.DayStart
{
    public class DayStartCalculator
    {
        public bool IsLeavingCompany(WorkerRuntime worker)
        {
            if (worker.IsBusy()) return false;
            return worker.Loyalty < Random.Range(1, 100);
        }
        
        public void DayStartWorker(WorkerRuntime workerRuntime)
        {
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    trait.OnStartOfDay(workerRuntime);
            
            workerRuntime.ResetDrankCoffeeToday();
            workerRuntime.ResetTookBreakToday();
            workerRuntime.TickLastBreakDay();
        }
        
        public void DayStartWorkerProductivity(WorkerRuntime workerRuntime, ProductivityTickCalculator calculator)
        {
            // Productivity Tick.
            calculator.ProductivityTick(workerRuntime); // Уникальная логика для черт учитывается.
            
            // Productivity Mod.
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    workerRuntime.SetProductivity(trait.OnStartOfDayProductivity(workerRuntime.Productivity));
        }

        public void OnDayStartTask()
        {
            if (!OfficeRuntime.Instance.CurrentTask)
                return;
            if (OfficeRuntime.Instance.CurrentTask.CurrentTaskDay != OfficeRuntime.Instance.CurrentTask.Task.Duration)
                return;
            
            TaskResultCalculator calculator = new TaskResultCalculator();
            
            TotalTaskResult totalTaskResult = calculator.TotalTaskResult(
                OfficeRuntime.Instance.CurrentTask,
                OfficeRuntime.Instance.CurrentTask.Workers
                );
            
            OfficeRuntime.Instance.CurrentTask.FinishTask();
            OfficeRuntime.Instance.SetTaskResult(totalTaskResult);
            OfficeRuntime.Instance.FreeActiveTask();
        }
    }
}