using Core.QuotaLogic;
using JetBrains.Annotations;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class QuotaResultData
    {
        public bool isSuccess;
        public Quota quota;
        public int quotaContribution;
        public double quotaCompletion;

        public void SetDataFromQuotaResult([CanBeNull] QuotaResult result)
        {
            if (!result) return;
            
            isSuccess = result.IsSuccess;
            quota = result.Quota;
            quotaContribution = result.QuotaContribution;
            quotaCompletion = result.QuotaCompletion;
        }
    }
}