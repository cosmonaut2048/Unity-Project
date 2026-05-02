using Runtime;
using UnityEngine;

namespace Core.DayLogic.TickCalculation
{
    public class ProductivityTickCalculator
    {
        // Если переопределена логика тика продуктивности -- используем её,
        // иначе делаем базовое списание.
        public void ProductivityTick(WorkerRuntime workerRuntime)
        {
            if (workerRuntime.Worker.PersonalityTraits != null) 
            {
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                {
                    if (trait.IsUniqueProductivityTick())
                    {
                        workerRuntime.SetProductivity(workerRuntime.Productivity - trait.ProductivityTickSize(workerRuntime));
                        return;
                    }
                }
                
            }
            workerRuntime.SetProductivity(workerRuntime.Productivity - workerRuntime.Worker.BaseProductivityTickSize);
        }
    }
}