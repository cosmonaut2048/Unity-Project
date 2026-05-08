using Core.QuotaLogic;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "QuotaRuntime", menuName = "Scriptable Objects/Runtime/QuotaRuntime")]
    public class QuotaRuntime : ScriptableObject
    {
        [SerializeField] private Quota quota;
        [SerializeField] private int quotaDay;
        [SerializeField] private int quotaProgressNew;
        [SerializeField] private int quotaProgressOld;

        public Quota StaticQuota => quota;
        public int QuotaProgressNew => quotaProgressNew;
        public int QuotaProgressOld => quotaProgressOld;
        public int QuotaDay => quotaDay;

        public void FillQuota(int newProgress)
        {
            quotaProgressNew = Mathf.Clamp(quotaProgressNew + newProgress, 0, quota.QuotaSize);
        }

        public void NewProgressToOld()
        {
            quotaProgressOld = Mathf.Clamp(quotaProgressOld + quotaProgressNew, 0, quota.QuotaSize);
        }

        public void SetQuotaRuntime(Quota newQuota, int progress, int oldProgress)
        {
            quota = newQuota;
            quotaProgressNew = progress;
            quotaProgressOld = oldProgress;
        }
        
        public void SetQuotaRuntime(Quota newQuota, int newQuotaDay, int progress, int oldProgress)
        {
            quota = newQuota;
            quotaDay = newQuotaDay;
            quotaProgressNew = progress;
            quotaProgressOld = oldProgress;
        }

        public void TickQuotaDay()
        {
            quotaDay++;
        }
    }
}