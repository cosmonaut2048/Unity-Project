using UnityEngine;

namespace Core.TaskLogic
{
    public class TotalTaskResult : ScriptableObject
    {
        [SerializeField] private bool isSuccess;
        [SerializeField] private bool isCriticalFailure;
        [SerializeField] private bool isCriticalSuccess;
        
        public bool IsSuccess => isSuccess;
        public bool IsCriticalFailure => isCriticalFailure;
        public bool IsCriticalSuccess => isCriticalSuccess;
        
        public void SetIsSuccess(bool success) =>  isSuccess = success;
        public void SetIsCriticalFailure(bool criticalFailure) =>  isCriticalFailure = criticalFailure;
        public void SetIsCriticalSuccess(bool criticalSuccess) =>  isCriticalSuccess = criticalSuccess;
    }
}