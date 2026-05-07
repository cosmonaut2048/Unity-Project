using System.Collections.Generic;
using Content;
using JetBrains.Annotations;
using Runtime;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class TaskRuntimeData
    {
        public TaskDef task;
        public List<ItemDef> gear;
        public List<WorkerRuntimeData> workers;
        public int currentTaskDay;
        public bool isFinished;
        public bool isEmpty;

        public void SetDataFromTaskRuntime([CanBeNull] TaskRuntime taskRuntime)
        {
            if (!taskRuntime)
            {
                isEmpty = true;
                return;
            }
            
            isEmpty = false;
            
            task = taskRuntime.Task;
            gear = taskRuntime.Gear;

            workers = new List<WorkerRuntimeData>();

            foreach (var worker in taskRuntime.Workers)
            {
                WorkerRuntimeData workerData = new WorkerRuntimeData();
                workerData.SetDataFromWorkerRuntime(worker);
                workers.Add(workerData);
            }

            currentTaskDay = taskRuntime.CurrentTaskDay;
            isFinished = taskRuntime.IsFinished;
        }
    }
}