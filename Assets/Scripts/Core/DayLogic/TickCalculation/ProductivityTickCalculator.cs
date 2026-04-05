using Runtime;
using Scriptable_Objects.Catalogs;

namespace Core.DayLogic
{
    public class ProductivityTickCalculator
    {
        // Если переопределена логика тика продуктивности -- используем её,
        // иначе делаем базовое списание.
        public void ProductivityTick(WorkerRuntime worker)
        {
            if (worker.PersonalityTraits != null) 
            {
                foreach (var trait in worker.PersonalityTraits)
                {
                    if (trait.IsUniqueProductivityTick())
                    {
                        worker.Productivity -= trait.ProductivityTickSize(worker);
                        return;
                    }
                }
                
            }
            worker.Productivity -= worker.BaseProductivityTickSize;
        }
    }
}