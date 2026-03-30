using UnityEngine;

namespace Core.QuotaLogic
{
    [CreateAssetMenu(fileName = "Quota", menuName = "Scriptable Objects/QuotaLogic/Quota")]
    public class Quota : ScriptableObject
    {
        [Header("Basic info")]
        public string quotaName;
        public string quotaDescription;
        public int quotaSize;

        public Quota(string quotaName, string quotaDescription, int quotaSize)
        {
            this.quotaName = quotaName;
            this.quotaDescription = quotaDescription;
            this.quotaSize = quotaSize;
        }
    }
}