using System.Collections.Generic;
using Runtime;
using Random = UnityEngine.Random;

namespace Core.DayLogic
{
    public class DayStartCalculator
    {
        public bool IsLeavingCompany(WorkerRuntime worker)
        {
            return worker.Loyalty < Random.Range(1, 100);
        }
        
        public void DayStartWorker(WorkerRuntime worker)
        {
            if (worker.PersonalityTraits != null)
                foreach (var trait in worker.PersonalityTraits)
                    trait.OnStartOfDay(worker);
        }
        
        public void DayStartWorkerProductivity(WorkerRuntime worker, ProductivityTickCalculator calculator)
        {
            // Productivity Tick.
            calculator.ProductivityTick(worker); // Уникальная логика для черт учитывается.
            
            // Productivity Mod.
            if (worker.PersonalityTraits != null)
                foreach (var trait in worker.PersonalityTraits)
                    worker.Productivity = trait.OnStartOfDayProductivity(worker.Productivity);
        }
    }
}