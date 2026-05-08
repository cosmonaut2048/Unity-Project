using JetBrains.Annotations;
using Runtime;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class QuotaRuntimeData
    {
        public QuotaData quotaData;
        public int quotaDay;
        public int quotaProgressNew;
        public int quotaProgressOld;

        public void SetDataFromQuotaRuntime([CanBeNull] QuotaRuntime quota)
        {
            quotaData = new QuotaData();
            if (!quota) return;
            
            quotaData.SetDataFromQuota(quota.StaticQuota);

            quotaDay = quota.QuotaDay;
            quotaProgressNew = quota.QuotaProgressNew;
            quotaProgressOld = quota.QuotaProgressOld;
        }
    }
}