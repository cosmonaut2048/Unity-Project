using Core.QuotaLogic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace Runtime
{
    [CreateAssetMenu(fileName = "QuotaRuntime", menuName = "Scriptable Objects/Runtime/QuotaRuntime")]
    public class QuotaRuntime : ScriptableObject
    {
        [SerializeField] private Quota quota;
        [SerializeField] private int quotaProgressNew;
        [SerializeField] private int quotaProgressOld;

        public Quota StaticQuota => quota;
        public int QuotaProgressNew => quotaProgressNew;
        public int QuotaProgressOld => quotaProgressOld;

        public void FillQuota(int progress)
        {
            quotaProgressNew = Mathf.Clamp(quotaProgressNew + progress, 0, quotaProgressNew);
        }
    }
}