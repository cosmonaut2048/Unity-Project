using Content;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class WorkerRuntimeData
    {
        public WorkerDefData workerDefData;

        public bool isEmployed = true;
        public int productivity = 100;
        public int loyalty = 100;
        public int lastBreakDay;
        
        public BusyReason busyReason = BusyReason.None;
        public bool isProductivityFrozen;
        public bool isLoyaltyFrozen;
        
        public bool drankCoffeeToday;
        public bool tookBreakToday;
        
        public void SetDataFromWorkerRuntime([CanBeNull] WorkerRuntime worker)
        {
            workerDefData = new WorkerDefData();
            if (!worker) return;
            
            workerDefData.SetDataFromWorkerDef(worker.Worker);

            isEmployed = worker.IsEmployed;
            productivity = worker.Productivity;
            loyalty = worker.Loyalty;
            lastBreakDay = worker.LastBreakDay;
            isProductivityFrozen = worker.IsProductivityFrozen;
            isLoyaltyFrozen = worker.IsLoyaltyFrozen;
            drankCoffeeToday = worker.DrankCoffeeToday;
            tookBreakToday = worker.TookBreakToday;
        }
    }
}