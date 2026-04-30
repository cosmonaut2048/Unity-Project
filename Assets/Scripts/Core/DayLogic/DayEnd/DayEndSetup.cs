using Content;
using Core.DayLogic.ReportLogic;
using Core.DayLogic.TickCalculation;
using Runtime;
using UnityEngine;

namespace Core.DayLogic.DayEnd
{
    public class DayEndSetup : MonoBehaviour
    {
        public void SetupDayEnd()
        {
            DayEndCalculator dayEndCalculator = new DayEndCalculator();
            LoyaltyTickCalculator loyaltyCalculator = new LoyaltyTickCalculator();
            DailyReportCreator reportCreator = new DailyReportCreator();

            foreach (var worker in OfficeRuntime.Instance.HiredWorkers)
            {
                dayEndCalculator.DayEndWorker(worker);
                dayEndCalculator.DayEndWorkerLoyalty(worker, loyaltyCalculator);
            }
            
            OfficeRuntime.Instance.SetDailyReport(reportCreator.CreateReport());
            dayEndCalculator.OnDayEndTask();
        }
    }
}