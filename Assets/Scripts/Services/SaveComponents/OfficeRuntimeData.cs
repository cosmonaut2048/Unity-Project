using System.Collections.Generic;
using Content;
using Core.QuotaLogic;
using Runtime;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class OfficeRuntimeData
    {
        public List<ItemDef> inventory =  new List<ItemDef>();
        public int dayOfTheWeek;
        // Работники.
        public List<WorkerRuntimeData> hiredWorkers = new List<WorkerRuntimeData>();
        public List<WorkerRuntimeData> firedWorkersToday =  new List<WorkerRuntimeData>();
        // Квота.
        public QuotaRuntimeData currentQuota;
        // Задание.
        public TaskDef availableTask;
        public TaskRuntimeData currentTask;
        public TotalTaskResultData lastTaskResult;
        
        // Кофе.
        public int coffee;
        public int coffeeConsumedToday;
        public int coffeeObtainedToday;
        // Перерывы.
        public int breakVouchers;
        public int breakVouchersUsedToday;
        // Отчёты.
        public DailyReportData dailyReport;
        public QuotaResultData quotaResult;

        public void SetDataFromOffice()
        {
            inventory = OfficeRuntime.Instance.Inventory;
            dayOfTheWeek = OfficeRuntime.Instance.DayOfTheWeek;

            hiredWorkers = GetWorkerRuntimeDataList(OfficeRuntime.Instance.HiredWorkers);
            firedWorkersToday = GetWorkerRuntimeDataList(OfficeRuntime.Instance.FiredWorkersToday);
            
            currentQuota = new QuotaRuntimeData();
            currentQuota.SetDataFromQuotaRuntime(OfficeRuntime.Instance.CurrentQuota);
            
            availableTask = OfficeRuntime.Instance.AvailableTask;

            currentTask = new TaskRuntimeData();
            currentTask.SetDataFromTaskRuntime(OfficeRuntime.Instance.CurrentTask);
            
            lastTaskResult = new TotalTaskResultData();
            lastTaskResult.SetDataFromTaskResult(OfficeRuntime.Instance.LastTaskResult);
            
            coffee = OfficeRuntime.Instance.Coffee;
            coffeeConsumedToday = OfficeRuntime.Instance.CoffeeConsumedToday;
            coffeeObtainedToday = OfficeRuntime.Instance.CoffeeObtainedToday;
            
            breakVouchers = OfficeRuntime.Instance.BreakVouchers;
            breakVouchersUsedToday = OfficeRuntime.Instance.BreakVouchersUsedToday;

            dailyReport = new DailyReportData();
            dailyReport.SetDataFromReport(OfficeRuntime.Instance.DailyReport);

            quotaResult = new QuotaResultData();
            quotaResult.SetDataFromQuotaResult(OfficeRuntime.Instance.QuotaResult);
        }

        private List<WorkerRuntimeData> GetWorkerRuntimeDataList(List<WorkerRuntime> workers)
        {
            List<WorkerRuntimeData> workerDataList = new List<WorkerRuntimeData>();
            
            foreach (var worker in workers)
            {
                WorkerRuntimeData workerData = new WorkerRuntimeData();
                workerData.SetDataFromWorkerRuntime(worker);
                workerDataList.Add(workerData);
            }
            
            return workerDataList;
        }
        
        // public void SetFromGameData(GameData gameData)
        // {
        //     inventory = gameData.Inventory;
        //     dayOfTheWeek = gameData.DayOfTheWeek;
        //     
        //     hiredWorkers = gameData.HiredWorkers;
        //     firedWorkersToday = gameData.FiredWorkers;
        //     
        //     currentQuota = gameData.CurrentQuota;
        //     
        //     availableTask = gameData.AvailableTask;
        //     currentTask = gameData.CurrentTask;
        //     lastTaskResult = gameData.LastTaskResult;
        //     
        //     coffee = gameData.Coffee;
        //     coffeeConsumedToday = gameData.CoffeeConsumedToday;
        //     coffeeObtainedToday = gameData.CoffeeObtainedToday;
        //     
        //     breakVouchers = gameData.BreakVouchers;
        //     breakVouchersUsedToday = gameData.BreakVouchersUsedToday;
        //     
        //     dailyReport = gameData.DailyReport;
        // }
    }
}