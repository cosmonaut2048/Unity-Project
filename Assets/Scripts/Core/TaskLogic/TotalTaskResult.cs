using System.Collections.Generic;
using Content;
using Runtime;
using UnityEngine;

namespace Core.TaskLogic
{
    public class TotalTaskResult : ScriptableObject
    {
        [SerializeField] private bool isSuccess;
        [SerializeField] private bool isCriticalFailure;
        [SerializeField] private bool isCriticalSuccess;
        [SerializeField] private List<WorkerRuntime> workers;
        [SerializeField] private TaskDef task;
        
        public bool IsSuccess => isSuccess;
        public bool IsCriticalFailure => isCriticalFailure;
        public bool IsCriticalSuccess => isCriticalSuccess;
        public  List<WorkerRuntime> Workers => workers;
        public TaskDef Task => task;
        
        public void SetIsSuccess(bool success) =>  isSuccess = success;
        public void SetIsCriticalFailure(bool criticalFailure) =>  isCriticalFailure = criticalFailure;
        public void SetIsCriticalSuccess(bool criticalSuccess) =>  isCriticalSuccess = criticalSuccess;
        public void SetWorkers(List<WorkerRuntime> assignedWorkers) => workers = assignedWorkers;
        public void SetTask(TaskDef completedTask) => task = completedTask;
    }
}