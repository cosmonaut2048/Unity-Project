using Runtime;

namespace Core.DayLogic
{
    public class DayEndCalculator
    {
        public void DayEndWorker(WorkerRuntime worker)
        {
            if (worker.PersonalityTraits != null)
                foreach (var trait in worker.PersonalityTraits)
                    trait.OnEndOfDay(worker);
        }

        public void DayEndWorkerLoyalty(WorkerRuntime worker, LoyaltyTickCalculator calculator)
        {
            // Loyalty Tick.
            calculator.LoyaltyTick(worker); // Уникальная логика для черт учитывается.
            
            // Loyalty Mod.
            if (worker.PersonalityTraits != null)
                foreach (var trait in worker.PersonalityTraits)
                    worker.SetLoyalty = trait.OnEndOfDayLoyalty(worker.Loyalty);
        }
    }
}