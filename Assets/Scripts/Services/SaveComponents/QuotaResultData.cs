using Core.QuotaLogic;
using JetBrains.Annotations;
using Runtime;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class QuotaResultData
    {
        public bool isSuccess;
        public QuotaData quota;
        public int quotaContribution;
        public double quotaCompletion;

        public void SetDataFromQuotaResult([CanBeNull] QuotaResult result)
        {
            if (!result) return;

            QuotaData quotaData = new QuotaData();
            quotaData.SetDataFromQuota(OfficeRuntime.Instance.CurrentQuota.StaticQuota);
            
            isSuccess = result.IsSuccess;
            quota = quotaData;
            quotaContribution = result.QuotaContribution;
            quotaCompletion = result.QuotaCompletion;
        }
    }
}