using System;
using System.Collections.Generic;
using UnityEngine;
using Content;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Worker", menuName = "Scriptable Objects/Runtime/Worker")]
    public class WorkerRuntime : WorkerDef
    {
        // Worker State.
        [SerializeField] private bool isEmployed = true;
        [SerializeField] private int productivity = 100;
        [SerializeField] private int loyalty = 100;
        [SerializeField] private int lastBreakDay;
        
        [SerializeField] private BusyReason busyReason = BusyReason.None;
        [SerializeField] private bool isProductivityFrozen;
        [SerializeField] private bool isLoyaltyFrozen;
        
        
        // Пограничные значения Productivity и Loyalty.
        private readonly int _productivityMinValue = 0;
        private readonly int _productivityMaxValue = 200;
        private readonly int _loyaltyMinValue = 0;
        private readonly int _loyaltyMaxValue = 100;
        
        // Свойства.
        public bool IsEmployed { get; set; }

        public int Productivity => productivity;
        public int SetProductivity
        { 
            set => productivity = Mathf.Clamp(isProductivityFrozen ? productivity : value, _productivityMinValue, _productivityMaxValue);
        }

        public int Loyalty => loyalty;
        public int SetLoyalty
        {
            set => loyalty = Mathf.Clamp(isLoyaltyFrozen ? loyalty : value, _loyaltyMinValue, _loyaltyMaxValue);
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
        
        // Методы.
        public bool IsBusy => busyReason != BusyReason.None;
        
        // Инициализация из WorkerDef.
        public void InitializeWorkerRuntime(
            WorkerAppearance newAppearance, 
            int patience, 
            int social, 
            int intellectual, 
            int physical, 
            List<TraitDef> traits)
        {
            // Вызываем метод базового класса.
            InitializeWorkerDef(newAppearance, patience, social, intellectual, physical, traits);
        }
        
        public void InitializeWorkerRuntime(WorkerDef worker)
        {
            // Вызываем метод базового класса.
            InitializeWorkerDef(worker.Appearance, worker.BasePatience, worker.BaseSocial, worker.BaseIntellectual, worker.BasePhysical, worker.PersonalityTraits);
        }
        
        // Больше не используется:
        
        [Obsolete("ChangeProductivity больше не используется -- используется свойство Productivity.")]
        public void ChangeProductivity(int delta)
        {
            if (isProductivityFrozen)
                return;
        
            productivity = Mathf.Clamp(productivity + delta, _productivityMinValue, _productivityMaxValue);
        }

        [Obsolete("ChangeLoyalty больше не используется -- используется свойство Loyalty.")]
        public void ChangeLoyalty(int delta)
        {
            if (isLoyaltyFrozen)
                return;
        
            loyalty = Mathf.Clamp(loyalty + delta, _loyaltyMinValue, _loyaltyMaxValue);
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