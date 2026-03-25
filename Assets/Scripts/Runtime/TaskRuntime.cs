using System.Collections.Generic;
using Content;
using JetBrains.Annotations;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "TaskRuntime", menuName = "Scriptable Objects/Runtime/TaskRuntime")]
    public class TaskRuntime : TaskDef
    {
        [Header("Gear")] // Снаряжение -- выбирает игрок.
        [CanBeNull] public List<ItemDef> gear;
        
        [Header("Workers")] // Работники, которых послали на задание.
        public List<WorkerDef> workers;

        public bool HasValidWorkerCount()
        {
            if (workers.Count == 0)
                return false;
            return workers.Count >= workerAmountRequired && MaxWorkerAmount >= workerAmountRequired;
        }
    }
}