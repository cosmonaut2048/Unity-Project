using UnityEngine;

namespace Core.QuotaLogic
{
    [CreateAssetMenu(fileName = "Quota", menuName = "Scriptable Objects/QuotaLogic/Quota")]
    public class Quota : ScriptableObject
    {
        [Header("Basic info")]
        [SerializeField] private string quotaName;
        [SerializeField] private string quotaDescription;
        [SerializeField] private int quotaSize;
        
        public string QuotaName => quotaName;
        public string QuotaDescription => quotaDescription;
        public int QuotaSize => quotaSize;

        public void InitializeQuota(string newQuotaName, string newQuotaDescription, int newQuotaSize)
        {
            quotaName = newQuotaName;
            quotaDescription = newQuotaDescription;
            quotaSize = newQuotaSize;
        }
    }
}