using System.Collections.Generic;
using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "TaskDef", menuName = "Scriptable Objects/Content/TaskDef")]
    public class TaskDef : ScriptableObject
    {
        [SerializeField] private int maxWorkerAmount = 4;
        
        [Header("Name and description")]
        [SerializeField] private string taskName;
        [SerializeField] private string taskDescription;

        [Header("Time and workers needed")]
        [SerializeField] private int duration; // Длина задания (в днях).
        [SerializeField] private int workerAmountRequired; // Минимум работников.
        
        [Header("Required skills")] // Пороги проверки.
        [SerializeField] private int patienceRequired;
        [SerializeField] private int socialRequired;
        [SerializeField] private int intellectualRequired;
        [SerializeField] private int physicalRequired;
        
        [Header("Reward(s)")] // Награда (предметы).
        [SerializeField] private List<ItemDef> reward;
        
        public int MaxWorkerAmount => maxWorkerAmount;
        public string TaskName => taskName;
        public string TaskDescription => taskDescription;
        public int Duration => duration;
        public int WorkerAmountRequired => workerAmountRequired;
        public int PatienceRequired => patienceRequired;
        public int SocialRequired => socialRequired;
        public int IntellectualRequired => intellectualRequired;
        public int PhysicalRequired => physicalRequired;
        public List<ItemDef> Reward => reward;
    }
}