using System.Collections.Generic;
using Content;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "TaskRuntime", menuName = "Scriptable Objects/Runtime/TaskRuntime")]
    public class TaskRuntime : ScriptableObject
    {
        [SerializeField] private TaskDef task;
        // Снаряжение -- выбирает игрок.
        [SerializeField] private List<ItemDef> gear;
        // Работники, которых послали на задание.
        [SerializeField] private List<WorkerRuntime> workers;
        
        [SerializeField] private int currentTaskDay;
        [SerializeField] private bool isFinished;
        
        public List<ItemDef> Gear => gear;
        public List<WorkerRuntime> Workers => workers;
        public int CurrentTaskDay => currentTaskDay;
        public bool IsFinished => isFinished;
        public TaskDef Task => task;

        public void InitializeTaskRuntime(TaskDef assignedTask, List<WorkerRuntime> assignedWorkers)
        {
            task = assignedTask;
            workers = assignedWorkers;
            currentTaskDay = 0;
            isFinished = false;
        }

        public bool HasValidWorkerCount()
        {
            if (workers.Count == 0)
                return false;
            return workers.Count >= task.WorkerAmountRequired && task.MaxWorkerAmount >= task.WorkerAmountRequired;
        }

        public void TickTaskDay()
        {
            currentTaskDay++;
        }

        public void FinishTask()
        {
            isFinished = true;
        }
    }
}