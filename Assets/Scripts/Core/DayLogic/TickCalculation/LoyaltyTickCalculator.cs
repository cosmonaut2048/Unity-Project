using Runtime;
using UnityEngine;

namespace Core.DayLogic.TickCalculation
{
    public class LoyaltyTickCalculator

    {
    // Если переопределена логика тика преданности -- используем её,
    // иначе делаем базовое списание.
    public void LoyaltyTick(WorkerRuntime workerRuntime)
    {
        if (workerRuntime.Worker.PersonalityTraits != null)
        {
            foreach (var trait in workerRuntime.Worker.PersonalityTraits)
            {
                if (trait.IsUniqueLoyaltyTick())
                {
                    workerRuntime.SetLoyalty(workerRuntime.Loyalty - trait.LoyaltyTickSize(workerRuntime));
                    return;
                }
            }
        }

        workerRuntime.SetLoyalty(workerRuntime.Loyalty - workerRuntime.Worker.BaseLoyaltyTickSize);
    }
    }
}