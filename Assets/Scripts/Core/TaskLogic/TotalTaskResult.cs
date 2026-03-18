using UnityEngine;

namespace Core.TaskLogic
{
    public class TotalTaskResult
    {
        private bool _isSuccess;
        private bool _isCriticalFailure;
        private bool _isCriticalSuccess;
        
        public bool IsSuccess { get; set; }
        public bool IsCriticalFailure { get; set; }
        public bool IsCriticalSuccess { get; set; }
    }
}