using Core.DayLogic.TickCalculation;
using Core.QuotaLogic;
using Runtime;
using UnityEngine;

namespace Core.DayLogic.DayEnd
{
    public class DayEndCalculator
    {
        public void DayEndWorker(WorkerRuntime workerRuntime)
        {
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    trait.OnEndOfDay(workerRuntime);
        }

        public void DayEndWorkerLoyalty(WorkerRuntime workerRuntime, LoyaltyTickCalculator calculator)
        {
            // Loyalty Tick.
            calculator.LoyaltyTick(workerRuntime); // Уникальная логика для черт учитывается.
            
            // Loyalty Mod.
            if (workerRuntime.Worker.PersonalityTraits != null)
                foreach (var trait in workerRuntime.Worker.PersonalityTraits)
                    workerRuntime.SetLoyalty(trait.OnEndOfDayLoyalty(workerRuntime.Loyalty));
        }

        public void OnDayEndTask()
        {
            if (!OfficeRuntime.Instance.CurrentTask)
                return;
            if (OfficeRuntime.Instance.CurrentTask.CurrentTaskDay < OfficeRuntime.Instance.CurrentTask.Task.Duration)
                OfficeRuntime.Instance.CurrentTask.TickTaskDay();
        }

        public void OnDayEndQuota()
        {
            QuotaCalculator calculator = new QuotaCalculator();

            OfficeRuntime.Instance.CurrentQuota.FillQuota(calculator.TotalQuotaContribution(
                OfficeRuntime.Instance.WorkersInOffice()));
            
            if (OfficeRuntime.Instance.CurrentQuota.QuotaDay ==
                OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaDuration)
            {
                bool isSuccess = OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaSize == 
                                 OfficeRuntime.Instance.CurrentQuota.QuotaProgressNew;
                
                QuotaResult result = ScriptableObject.CreateInstance<QuotaResult>();
                result.InitializeQuotaResult(
                    isSuccess, 
                    OfficeRuntime.Instance.CurrentQuota.StaticQuota,
                    OfficeRuntime.Instance.CurrentQuota.QuotaProgressNew);
                
                OfficeRuntime.Instance.SetQuotaResult(result);
            }
        }
    }
}