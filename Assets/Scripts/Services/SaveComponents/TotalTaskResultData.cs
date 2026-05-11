using System.Collections.Generic;
using Content;
using Core.TaskLogic;
using JetBrains.Annotations;
using Runtime;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class TotalTaskResultData
    {
        public bool isSuccess;
        public bool isCriticalFailure;
        public bool isCriticalSuccess;
        public List<WorkerRuntimeData> workers = new List<WorkerRuntimeData>();
        public TaskDefData task;

        public bool isEmpty;

        public void SetDataFromTaskResult([CanBeNull] TotalTaskResult taskResult)
        {
            if (!taskResult)
            {
                isEmpty = true;
                return;
            }
            
            isEmpty = false;
            
            isSuccess = taskResult.IsSuccess;
            isCriticalFailure = taskResult.IsCriticalFailure;
            isCriticalSuccess = taskResult.IsCriticalSuccess;

            foreach (var worker in taskResult.Workers)
            {
                WorkerRuntimeData workerData = new WorkerRuntimeData();
                workerData.SetDataFromWorkerRuntime(worker);
                workers.Add(workerData);
            }
            
            task.taskName = taskResult.Task ? taskResult.Task.name : "";
        }
    }
}