using Runtime;

namespace Core.DayLogic
{
    public class DayEndCalculator
    {
        public void DayEndWorker(WorkerRuntime worker)
        {
            if (worker.personalityTraits != null)
                foreach (var trait in worker.personalityTraits)
                    trait.OnEndOfDay(worker);
        }

        public void DayEndLoyalty(WorkerRuntime worker, LoyaltyTickCalculator calculator)
        {
            // Loyalty Tick.
            calculator.LoyaltyTick(worker); // Уникальная логика для черт учитывается.
            
            // Loyalty Mod.
            if (worker.personalityTraits != null)
                foreach (var trait in worker.personalityTraits)
                    worker.Loyalty = trait.OnEndOfDayLoyalty(worker.Loyalty);
        }
    }
}