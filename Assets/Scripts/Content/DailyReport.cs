using System.Collections.Generic;
using Runtime;
using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "DailyReport", menuName = "Scriptable Objects/Content/DailyReport")]
    public class DailyReport : ScriptableObject
    {
        [SerializeField] private List<WorkerRuntime> workers;

        [SerializeField] private float quotaProgressOld;
        [SerializeField] private float quotaProgressNew;
        [SerializeField] private float quotaSize;

        [SerializeField] private int coffeeConsumed;
        [SerializeField] private int coffeeObtained;
        [SerializeField] private int coffeeLeft;
        
        [SerializeField] private int breaksTaken;
        [SerializeField] private int breaksLeft;
        
        [SerializeField] private int daysLeft;
        
        public List<WorkerRuntime> Workers => workers;
        
        public float QuotaProgressOld => quotaProgressOld;
        public float QuotaProgressNew => quotaProgressNew;
        public float QuotaSize => quotaSize;
        
        public int CoffeeConsumed => coffeeConsumed;
        public int CoffeeObtained => coffeeObtained;
        public int CoffeeLeft => coffeeLeft;
        
        public int BreaksTaken => breaksTaken;
        public int BreaksLeft => breaksLeft;
        public int DaysLeft => daysLeft;
    }
}