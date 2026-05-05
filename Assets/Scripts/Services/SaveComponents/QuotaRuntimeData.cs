using JetBrains.Annotations;
using Runtime;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class QuotaRuntimeData
    {
        public QuotaData quotaData;
        public int quotaProgressNew;
        public int quotaProgressOld;

        public void SetDataFromQuotaRuntime([CanBeNull] QuotaRuntime quota)
        {
            quotaData = new QuotaData();
            if (!quota) return;
            
            quotaData.SetDataFromQuota(quota.StaticQuota);

            quotaProgressNew = quota.QuotaProgressNew;
            quotaProgressOld = quota.QuotaProgressOld;
        }
    }
}