using Core.DayLogic.TickCalculation;
using Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.DayLogic.DayStart
{
    public class DayStartCalculator
    {
        public bool IsLeavingCompany(WorkerRuntime worker)
        {
            return worker.Loyalty < Random.Range(1, 100);
        }
        
        public void DayStartWorker(WorkerRuntime workerRuntime)
        {
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    trait.OnStartOfDay(workerRuntime);
        }
        
        public void DayStartWorkerProductivity(WorkerRuntime workerRuntime, ProductivityTickCalculator calculator)
        {
            // Productivity Tick.
            calculator.ProductivityTick(workerRuntime); // Уникальная логика для черт учитывается.
            
            // Productivity Mod.
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    workerRuntime.SetProductivity = trait.OnStartOfDayProductivity(workerRuntime.Productivity);
        }
    }
}