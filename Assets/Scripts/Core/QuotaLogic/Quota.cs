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
        [SerializeField] private int quotaDuration = 5;
        
        private readonly int _minDuration = 3;
        private readonly int _maxDuration = 5;
        
        public int MaxDuration => _maxDuration;
        
        public string QuotaName => quotaName;
        public string QuotaDescription => quotaDescription;
        public int QuotaSize => quotaSize;
        public int QuotaDuration => quotaDuration;

        public void InitializeQuota(
            string newQuotaName, 
            string newQuotaDescription, 
            int newQuotaSize, 
            int newQuotaDuration)
        {
            quotaName = newQuotaName;
            quotaDescription = newQuotaDescription;
            quotaSize = newQuotaSize;
            quotaDuration = Mathf.Clamp(newQuotaDuration, _minDuration, _maxDuration);
        }
    }
}