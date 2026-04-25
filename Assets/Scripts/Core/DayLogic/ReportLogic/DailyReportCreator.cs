using System.Collections.Generic;
using System.Linq;
using Content;
using Runtime;
using UnityEngine;

namespace Core.DayLogic.ReportLogic
{
    public class DailyReportCreator
    {
        public DailyReport CreateReport()
        {
            List<WorkerRuntime> hiredTemp = OfficeRuntime.Instance.HiredWorkers;
            List<WorkerRuntime> firedTemp = OfficeRuntime.Instance.FiredWorkersToday;
            hiredTemp.AddRange(firedTemp);
            List<WorkerRuntime> workersInReport = hiredTemp.OrderBy(w => w.Worker.Appearance.WorkerName).ToList();
            
            DailyReport report = ScriptableObject.CreateInstance<DailyReport>();
            report.InitializeDailyReport(
                workersInReport,
                OfficeRuntime.Instance.CurrentQuota.QuotaProgressOld,
                OfficeRuntime.Instance.CurrentQuota.QuotaProgressNew,
                OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaSize,
                OfficeRuntime.Instance.CoffeeConsumedToday,
                OfficeRuntime.Instance.CoffeeObtainedToday,
                OfficeRuntime.Instance.Coffee,
                OfficeRuntime.Instance.BreakVouchersUsedToday,
                OfficeRuntime.Instance.BreakVouchers,
                5 - OfficeRuntime.Instance.DayOfTheWeek
                );
            
            return report;
        }
    }
}