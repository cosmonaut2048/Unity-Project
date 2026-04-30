using Core.DayLogic.TickCalculation;
using Runtime;

namespace Core.DayLogic.DayEnd
{
    public class DayEndCalculator
    {
        public void DayEndWorker(WorkerRuntime workerRuntime)
        {
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    trait.OnEndOfDay(workerRuntime);
        }

        public void DayEndWorkerLoyalty(WorkerRuntime workerRuntime, LoyaltyTickCalculator calculator)
        {
            // Loyalty Tick.
            calculator.LoyaltyTick(workerRuntime); // Уникальная логика для черт учитывается.
            
            // Loyalty Mod.
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    workerRuntime.SetLoyalty = trait.OnEndOfDayLoyalty(workerRuntime.Loyalty);
        }

        public void OnDayEndTask()
        {
            if (!OfficeRuntime.Instance.CurrentTask)
                return;
            if (OfficeRuntime.Instance.CurrentTask.CurrentTaskDay < OfficeRuntime.Instance.CurrentTask.Task.Duration)
                OfficeRuntime.Instance.CurrentTask.TickTaskDay();
        }
    }
}