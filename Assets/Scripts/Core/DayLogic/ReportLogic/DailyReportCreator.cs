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
            var workersInReport = OfficeRuntime.Instance.HiredWorkers
                .Concat(OfficeRuntime.Instance.FiredWorkersToday)
                .OrderBy(w => w.Worker.Appearance.WorkerName)
                .ToList();
            
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