using JetBrains.Annotations;
using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "TaskDef", menuName = "Scriptable Objects/Content/TaskDef")]
    public class TaskDef : ScriptableObject
    {
        [SerializeField] private int maxWorkerAmount = 4;
        
        [Header("Name and description")]
        public string taskName;
        public string taskDescription;

        [Header("Time and workers needed")]
        public int duration;
        public int workerAmountRequired;
        
        [Header("Required skills")]
        public int patienceRequired;
        public int socialRequired;
        public int intellectualRequired;
        public int physicalRequired;
        
        [Header("Reward(s)")]
        [CanBeNull] public OfficeItem[] reward;
        
        [Header("Gear")]
        [CanBeNull] public OfficeItem[] gear; // Gear is chosen by player from inventory
    }
}