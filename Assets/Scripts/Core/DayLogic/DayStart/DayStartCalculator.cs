using System.Collections.Generic;
using Content;
using Core.DayLogic.TickCalculation;
using Core.TaskLogic;
using Generators;
using Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.DayLogic.DayStart
{
    public class DayStartCalculator
    {
        public bool IsLeavingCompany(WorkerRuntime worker)
        {
            if (worker.IsBusy()) return false;
            return worker.Loyalty < Random.Range(1, 100);
        }
        
        public void DayStartWorker(WorkerRuntime workerRuntime)
        {
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    trait.OnStartOfDay(workerRuntime);
            
            workerRuntime.ResetDrankCoffeeToday();
            workerRuntime.ResetTookBreakToday();
            workerRuntime.TickLastBreakDay();
        }
        
        public void DayStartWorkerProductivity(WorkerRuntime workerRuntime, ProductivityTickCalculator calculator)
        {
            // Productivity Tick.
            calculator.ProductivityTick(workerRuntime); // Уникальная логика для черт учитывается.
            
            // Productivity Mod.
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    workerRuntime.SetProductivity(trait.OnStartOfDayProductivity(workerRuntime.Productivity));
        }

        public void OnDayStartTask()
        {
            if (!OfficeRuntime.Instance.CurrentTask)
                return;
            if (OfficeRuntime.Instance.CurrentTask.CurrentTaskDay != OfficeRuntime.Instance.CurrentTask.Task.Duration)
                return;
            
            TaskResultCalculator calculator = new TaskResultCalculator();
            
            TotalTaskResult totalTaskResult = calculator.TotalTaskResult(
                OfficeRuntime.Instance.CurrentTask,
                OfficeRuntime.Instance.CurrentTask.Workers
                );
            
            OfficeRuntime.Instance.CurrentTask.FinishTask();
            OfficeRuntime.Instance.SetTaskResult(totalTaskResult);
            OnTaskResult(
                totalTaskResult.IsSuccess, 
                totalTaskResult.IsCriticalSuccess, 
                totalTaskResult.IsCriticalFailure,
                totalTaskResult.Task,
                totalTaskResult.Workers);
            OfficeRuntime.Instance.FreeActiveTask();
        }

        private void OnTaskResult(bool isSuccess, bool isCriticalSuccess, bool isCriticalFailure, TaskDef task, List<WorkerRuntime> workers)
        {
            if (isSuccess)
            {
                OfficeRuntime.Instance.SetCoffee(OfficeRuntime.Instance.Coffee + task.CoffeeReward);
                OfficeRuntime.Instance.SetCoffeeObtainedToday(OfficeRuntime.Instance.CoffeeObtainedToday + task.CoffeeReward);
                
                OfficeRuntime.Instance.SetBreakVouchers(OfficeRuntime.Instance.BreakVouchers + task.VouchersReward);
            }

            foreach (var worker in workers)
            {
                if (isCriticalSuccess)
                    worker.SetProductivity(worker.Productivity * 2);
                
                if (isCriticalFailure)
                    worker.SetProductivity(worker.Productivity / 2);
            }
        }

        public void OnDayStartQuota()
        {
            if (!OfficeRuntime.Instance.CurrentQuota)
            {
                QuotaGenerator generator = new QuotaGenerator();
                OfficeRuntime.Instance.SetCurrentQuota(generator.GenerateFirstQuotaRuntime());
                return;
            }

            if (OfficeRuntime.Instance.CurrentQuota.QuotaDay ==
                OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaDuration)
            {
                Debug.Log($"Quota {OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaName} is finished.");

                QuotaGenerator generator = new QuotaGenerator();
                
                if (!OfficeRuntime.Instance.QuotaResult)
                {
                    Debug.Log($"Quota result is null!");
                    Debug.Log($"Generating default quota.");
                    OfficeRuntime.Instance.SetCurrentQuota(generator.GenerateFirstQuotaRuntime());
                    return;
                }
                
                
                OfficeRuntime.Instance.SetCurrentQuota(
                    generator.GenerateQuotaRuntime(
                        OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaSize, 
                        OfficeRuntime.Instance.QuotaResult.IsSuccess));
                
                Debug.Log($"New quota {OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaName} is assigned.");
            }
            
            OfficeRuntime.Instance.CurrentQuota.TickQuotaDay();
        }
    }
}