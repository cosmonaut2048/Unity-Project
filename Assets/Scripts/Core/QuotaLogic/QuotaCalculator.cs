using System.Collections.Generic;
using Runtime;

namespace Core.QuotaLogic
{
    public class QuotaCalculator
    {
        public int TotalQuotaContribution(List<WorkerRuntime> workers)
        {
            int contribution = 0;
            
            foreach (var worker in workers)
                contribution += worker.Productivity;
            
            return contribution;
        }
    }
}