using System.Collections.Generic;
using Content;
using Core.QuotaLogic;
using Core.TaskLogic;
using Runtime;
using Services.SaveComponents;
using UnityEngine;

namespace Services
{
    public class DataSetter
    {
        public void SetOfficeFromGameData(GameData data)
        {
            OfficeRuntime.Instance.SetInventory(data.office.inventory);
            OfficeRuntime.Instance.SetDayOfTheWeek(data.office.dayOfTheWeek);

            OfficeRuntime.Instance.SetHiredWorkers(CreateWorkerRuntimeListFormData(data.office.hiredWorkers));
            OfficeRuntime.Instance.SetFiredWorkersToday(CreateWorkerRuntimeListFormData(data.office.firedWorkersToday));
            
            OfficeRuntime.Instance.SetCurrentQuota(CreateQuotaRuntimeFromData(data.office.currentQuota));
            
            OfficeRuntime.Instance.SetAvailableTask(data.office.availableTask);
            OfficeRuntime.Instance.SetCurrentTask(CreateTaskRuntimeFromData(data.office.currentTask));
            OfficeRuntime.Instance.SetTaskResult(CreateTaskResultFromData(data.office.lastTaskResult));
            
            OfficeRuntime.Instance.SetCoffee(data.office.coffee);
            OfficeRuntime.Instance.SetCoffeeConsumedToday(data.office.coffeeConsumedToday);
            OfficeRuntime.Instance.SetCoffeeObtainedToday(data.office.coffeeObtainedToday);
            
            OfficeRuntime.Instance.SetBreakVouchers(data.office.breakVouchers);
            OfficeRuntime.Instance.SetBreakVouchersUsedToday(data.office.breakVouchersUsedToday);
            
            OfficeRuntime.Instance.SetDailyReport(CreateDailyReportFromData(data.office.dailyReport));
        }

        private DailyReport CreateDailyReportFromData(DailyReportData data)
        {
            if (data.isEmpty)
                return null;
            
            DailyReport report = ScriptableObject.CreateInstance<DailyReport>();
            
            report.InitializeDailyReport(
                CreateWorkerRuntimeListFormData(data.workers),
                data.quotaProgressOld, data.quotaProgressNew, data.quotaSize,
                data.coffeeConsumed, data.coffeeObtained, data.coffeeLeft,
                data.breaksTaken, data.breaksLeft,
                data.daysLeft
                );
            
            return report;
        }

        private TotalTaskResult CreateTaskResultFromData(TotalTaskResultData data)
        {
            if (data.isEmpty)
                return null;
            
            TotalTaskResult result = ScriptableObject.CreateInstance<TotalTaskResult>();
            
            result.SetTotalTaskResult(
                data.isSuccess,
                data.isCriticalFailure,
                data.isCriticalSuccess,
                CreateWorkerRuntimeListFormData(data.workers),
                data.task
                );
            
            return result;
        }

        private TaskRuntime CreateTaskRuntimeFromData(TaskRuntimeData data)
        {
            if (data.isEmpty)
                return null;
            
            TaskRuntime taskRuntime = ScriptableObject.CreateInstance<TaskRuntime>();
            
            taskRuntime.SetTaskRuntime(
                data.task,
                data.gear,
                CreateWorkerRuntimeListFormData(data.workers),
                data.currentTaskDay,
                data.isFinished
                );
            
            return taskRuntime;
        }

        private QuotaRuntime CreateQuotaRuntimeFromData(QuotaRuntimeData data)
        {
            QuotaRuntime quotaRuntime = ScriptableObject.CreateInstance<QuotaRuntime>();
            
            quotaRuntime.SetQuotaRuntime(
                CreateQuotaFromData(data.quotaData),
                data.quotaProgressNew,
                data.quotaProgressOld
                );
            
            return quotaRuntime;
        }

        private Quota CreateQuotaFromData(QuotaData data)
        {
            Quota quota = ScriptableObject.CreateInstance<Quota>();
            quota.InitializeQuota(
                data.quotaName,
                data.quotaDescription,
                data.quotaSize
                );
            
            return quota;
        }

        private List<WorkerRuntime> CreateWorkerRuntimeListFormData(List<WorkerRuntimeData> data)
        {
            List<WorkerRuntime> workers = new List<WorkerRuntime>();

            foreach (WorkerRuntimeData workerData in data)
            {
                workers.Add(CreateWorkerRuntimeFormData(workerData));
            }
            
            return workers;
        }

        private WorkerRuntime CreateWorkerRuntimeFormData(WorkerRuntimeData data)
        {
            WorkerRuntime worker = ScriptableObject.CreateInstance<WorkerRuntime>();
            
            worker.SetWorkerRuntime(
                CreateWorkerDefFormData(data.workerDefData),
                data.isEmployed,
                data.productivity,
                data.loyalty,
                data.lastBreakDay,
                data.busyReason,
                data.isProductivityFrozen,
                data.isLoyaltyFrozen,
                data.drankCoffeeToday,
                data.tookBreakToday
                );
            
            return worker;
        }

        private WorkerDef CreateWorkerDefFormData(WorkerDefData data)
        {
            WorkerDef workerDef = ScriptableObject.CreateInstance<WorkerDef>();
            
            workerDef.InitializeWorkerDef
            (
                data.appearance,
                data.basePatience,
                data.baseSocial,
                data.baseIntellectual,
                data.basePhysical,
                data.personalityTraits
            );
            
            return workerDef;
        }
    }
}