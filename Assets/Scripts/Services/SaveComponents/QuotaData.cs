using Core.QuotaLogic;
using JetBrains.Annotations;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class QuotaData
    {
        public string quotaName;
        public string quotaDescription;
        public int quotaSize;
        
        public void SetDataFromQuota([CanBeNull] Quota quota)
        {
            if (!quota) return;
            
            quotaName = quota.QuotaName;
            quotaDescription = quota.QuotaDescription;
            quotaSize = quota.QuotaSize;
        }
    }
}