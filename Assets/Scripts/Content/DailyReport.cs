using System.Collections.Generic;
using Runtime;
using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "DailyReport", menuName = "Scriptable Objects/Content/DailyReport")]
    public class DailyReport : ScriptableObject
    {
        [SerializeField] private List<WorkerRuntime> workers;

        [SerializeField] private int quotaProgressOld;
        [SerializeField] private int quotaProgressNew;
        [SerializeField] private int quotaSize;

        [SerializeField] private int coffeeConsumed;
        [SerializeField] private int coffeeObtained;
        [SerializeField] private int coffeeLeft;
        
        [SerializeField] private int breaksTaken;
        [SerializeField] private int breaksLeft;
        
        [SerializeField] private int daysLeft;
        
        public List<WorkerRuntime> Workers => workers;
        
        public int QuotaProgressOld => quotaProgressOld;
        public int QuotaProgressNew => quotaProgressNew;
        public int QuotaSize => quotaSize;
        
        public int CoffeeConsumed => coffeeConsumed;
        public int CoffeeObtained => coffeeObtained;
        public int CoffeeLeft => coffeeLeft;
        
        public int BreaksTaken => breaksTaken;
        public int BreaksLeft => breaksLeft;
        public int DaysLeft => daysLeft;

        public void InitializeDailyReport(
            List<WorkerRuntime> workersReport, 
            int quotaProgressOldReport, int quotaProgressNewReport, int quotaSizeReport, 
            int coffeeConsumedReport, int coffeeObtainedReport, int coffeeLeftReport,
            int breaksTakenReport, int breaksLeftReport, int daysLeftReport
            )
        {
            workers = workersReport;
            quotaProgressOld = quotaProgressOldReport;
            quotaProgressNew = quotaProgressNewReport;
            quotaSize = quotaSizeReport;
            
            coffeeConsumed = coffeeConsumedReport;
            coffeeObtained = coffeeObtainedReport;
            coffeeLeft = coffeeLeftReport;
            
            breaksTaken = breaksTakenReport;
            breaksLeft = breaksLeftReport;
            daysLeft = daysLeftReport;
        }
    }
}