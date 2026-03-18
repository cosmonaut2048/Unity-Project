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
        [CanBeNull] public List<OfficeItem> gear;
        
        [Header("Workers")] // Работники, которых послали на задание.
        public List<WorkerDef> workers;
    }
}