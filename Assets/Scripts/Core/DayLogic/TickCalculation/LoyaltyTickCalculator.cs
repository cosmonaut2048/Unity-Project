using Runtime;

namespace Core.DayLogic
{
    public class LoyaltyTickCalculator
    {
        // Если переопределена логика тика преданности -- используем её,
        // иначе делаем базовое списание.
        public void LoyaltyTick(WorkerRuntime worker)
        {
            if (worker.personalityTraits != null)
            {
                foreach (var trait in worker.personalityTraits)
                {
                    if (trait.IsUniqueLoyaltyTick())
                    {
                        worker.Loyalty -= trait.LoyaltyTickSize(worker);
                        return;
                    }
                }
            }
            worker.Loyalty -= worker.BaseLoyaltyTickSize;
        }
    }
}