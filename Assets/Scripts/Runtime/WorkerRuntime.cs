using System.Collections.Generic;
using UnityEngine;
using Content;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Worker", menuName = "Scriptable Objects/Runtime/Worker")]
    public class WorkerRuntime : ScriptableObject
    {
        [SerializeField] private WorkerDef worker;
        // Worker State.
        [SerializeField] private bool isEmployed = true;
        [SerializeField] private int productivity = 100;
        [SerializeField] private int loyalty = 100;
        [SerializeField] private int lastBreakDay;
        
        [SerializeField] private BusyReason busyReason = BusyReason.None;
        [SerializeField] private bool isProductivityFrozen;
        [SerializeField] private bool isLoyaltyFrozen;
        
        [SerializeField] private bool drankCoffeeToday;
        [SerializeField] private bool tookBreakToday;
        
        
        // Пограничные значения Productivity и Loyalty.
        private readonly int _productivityMinValue = 0;
        private readonly int _productivityMaxValue = 200;
        private readonly int _loyaltyMinValue = 0;
        private readonly int _loyaltyMaxValue = 100;
        
        // Свойства.
        public int ProductivityMaxValue => _productivityMaxValue;
        public int LoyaltyMaxValue => _loyaltyMaxValue;
        public WorkerDef Worker => worker;
        public bool IsEmployed => isEmployed;

        public int Productivity => productivity;
        public void SetProductivity(int newProductivity)
        { 
            productivity = Mathf.Clamp(isProductivityFrozen ? productivity : newProductivity, _productivityMinValue, _productivityMaxValue);
        }

        public int Loyalty => loyalty;
        public void SetLoyalty(int newLoyalty)
        {
            loyalty = Mathf.Clamp(isLoyaltyFrozen ? loyalty : newLoyalty, _loyaltyMinValue, _loyaltyMaxValue);
        }

        public int LastBreakDay => lastBreakDay;
        public int SetLastBreakDay
        {
            set => lastBreakDay = Mathf.Max(0, value);
        }
        
        public BusyReason BusyReason => busyReason;
        public BusyReason SetBusyReason
        {
            set => busyReason = value;
        }
        
        public bool IsProductivityFrozen => isProductivityFrozen;
        public bool SetIsProductivityFrozen { set => isProductivityFrozen = value; }
        public bool IsLoyaltyFrozen => isLoyaltyFrozen;
        public bool SetIsLoyaltyFrozen { set => isLoyaltyFrozen = value; }
        
        public bool DrankCoffeeToday => drankCoffeeToday;
        public bool TookBreakToday => tookBreakToday;
        
        // Методы.
        public bool IsBusy() => busyReason != BusyReason.None;

        public void FireWorker()
        {
            Debug.Log($"{worker.Appearance.WorkerName} is fired.");
            isEmployed = false;
        }

        public void DrinkCoffee()
        {
            if (OfficeRuntime.Instance.Coffee > 0)
            {
                drankCoffeeToday = true;
                OfficeRuntime.Instance.TickCoffee();
                
                foreach (var trait in worker.PersonalityTraits)
                {
                    trait.OnCoffee(this);
                }
            }
        }

        public void TakeBreak()
        {
            if (OfficeRuntime.Instance.BreakVouchers > 0)
            {
                tookBreakToday = true;
                lastBreakDay = 0;
                OfficeRuntime.Instance.TickBreakVouchers();
                
                foreach (var trait in worker.PersonalityTraits)
                {
                    trait.OnBreak(this);
                }
            }
        }

        public void ResetDrankCoffeeToday()
        {
            drankCoffeeToday = false;
        }

        public void ResetTookBreakToday()
        {
            tookBreakToday = false;
        }

        public void TickLastBreakDay()
        {
            lastBreakDay++;
        }

        // Инициализация из WorkerDef.
        public void InitializeWorkerRuntime(
            WorkerAppearance newAppearance, 
            int patience, 
            int social, 
            int intellectual, 
            int physical, 
            List<TraitDef> traits)
        {
            worker.InitializeWorkerDef(newAppearance, patience, social, intellectual, physical, traits);
        }
        
        public void InitializeWorkerRuntime(WorkerDef workerDef)
        {
            worker =  workerDef;
        }
        
        // private int _freezeProductivityDays;
        // private int _freezeLoyaltyDays;
        
        // public int FreezeProductivityDays
        // {
        //     get => _freezeProductivityDays;
        //     set => _freezeProductivityDays = Mathf.Max(0, value);
        // }

        // public int FreezeLoyaltyDays
        // {
        //     get => _freezeLoyaltyDays;
        //     set => _freezeLoyaltyDays = Mathf.Max(0, value);
        // }
        // public void TickFreezes()
        // {
        //     _freezeProductivityDays = Mathf.Max(0, _freezeProductivityDays - 1);
        //     _freezeLoyaltyDays = Mathf.Max(0, _freezeLoyaltyDays - 1);
        // }
    }
}