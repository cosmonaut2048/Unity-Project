using System.Collections.Generic;
using Content;
using JetBrains.Annotations;
using Runtime;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class DailyReportData
    {
        public List<WorkerRuntimeData> workers = new List<WorkerRuntimeData>();

        public int quotaProgressOld;
        public int quotaProgressNew;
        public int quotaSize;

        public int coffeeConsumed;
        public int coffeeObtained;
        public int coffeeLeft;
        
        public int breaksTaken;
        public int breaksLeft;
        
        public int daysLeft;
        
        public bool isEmpty;

        public void SetDataFromReport([CanBeNull] DailyReport report)
        {
            if (!report)
            {
                isEmpty = true;
                return;
            }
            
            isEmpty = false;
            
            foreach (var worker in report.Workers)
            {
                WorkerRuntimeData workerData = new WorkerRuntimeData();
                workerData.SetDataFromWorkerRuntime(worker);
                workers.Add(workerData);
            }

            quotaProgressOld = report.QuotaProgressOld;
            quotaProgressNew = report.QuotaProgressNew;
            quotaSize = report.QuotaSize;

            coffeeConsumed = report.CoffeeConsumed;
            coffeeObtained = report.CoffeeObtained;
            coffeeLeft = report.CoffeeLeft;

            breaksTaken = report.BreaksTaken;
            breaksLeft = report.BreaksLeft;
            daysLeft = report.DaysLeft;
        }
    }
}