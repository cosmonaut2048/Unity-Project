using System.Collections.Generic;

namespace Core.TaskLogic
{
    public class WorkerTaskResult
    {
        private List<bool> _success;
        private int _criticalSuccessAmount;
        private int _criticalFailureAmount;
        
        public List<bool> Success { get; set; }
        public int CriticalSuccessAmount { get; set; }
        public int CriticalFailureAmount { get; set; }
    }
}