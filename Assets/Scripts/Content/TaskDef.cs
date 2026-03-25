using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "TaskDef", menuName = "Scriptable Objects/Content/TaskDef")]
    public class TaskDef : ScriptableObject
    {
        [SerializeField] private int maxWorkerAmount = 4;
        public int MaxWorkerAmount => maxWorkerAmount;
        
        [Header("Name and description")]
        public string taskName;
        public string taskDescription;

        [Header("Time and workers needed")]
        public int duration; // Длина задания (в днях).
        public int workerAmountRequired; // Минимум работников.
        
        [Header("Required skills")] // Пороги проверки.
        public int patienceRequired;
        public int socialRequired;
        public int intellectualRequired;
        public int physicalRequired;
        
        [Header("Reward(s)")] // Награда (предметы).
        [CanBeNull] public List<ItemDef> reward;
    }
}