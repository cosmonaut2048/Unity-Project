using UnityEngine;

namespace Core.QuotaLogic
{
    public class QuotaResult : ScriptableObject
    {
        [SerializeField] private bool isSuccess;
        [SerializeField] private Quota quota;
        [SerializeField] private int quotaContribution;
        [SerializeField] private double quotaCompletion; // размер_вклада / размер_квоты.

        public void InitializeQuotaResult(bool setIsSuccess, Quota setQuota, int setQuotaContribution)
        {
            isSuccess = setIsSuccess;
            quota = setQuota;
            quotaContribution = setQuotaContribution;
            quotaCompletion = (float)setQuotaContribution / setQuota.QuotaSize;
        }
        
        public bool IsSuccess => isSuccess;
        public Quota Quota => quota;
        public int QuotaContribution => quotaContribution;
        public double QuotaCompletion => quotaCompletion;
    }
}