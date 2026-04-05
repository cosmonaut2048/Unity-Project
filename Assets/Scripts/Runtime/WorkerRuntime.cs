using System;
using UnityEngine;
using Content;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Worker", menuName = "Scriptable Objects/Runtime/Worker")]
    public class WorkerRuntime : WorkerDef
    {
        // Worker State.
        private bool _isEmployed;
        private int _productivity = 100;
        private int _loyalty = 100;
        private int _lastBreakDay;
        
        private BusyReason _busyReason = BusyReason.None;
        private bool _isProductivityFrozen;
        private bool _isLoyaltyFrozen;
        
        
        // Пограничные значения Productivity и Loyalty.
        private readonly int _productivityMinValue = 0;
        private readonly int _productivityMaxValue = 200;
        private readonly int _loyaltyMinValue = 0;
        private readonly int _loyaltyMaxValue = 100;
        
        // Свойства.
        public bool IsEmployed { get; set; }

        public int Productivity
        {
            get => _productivity;
            set => _productivity = Mathf.Clamp(_isProductivityFrozen ? _productivity : value, _productivityMinValue, _productivityMaxValue);
        }

        public int Loyalty
        {
            get => _loyalty;
            set => _loyalty = Mathf.Clamp(_isLoyaltyFrozen ? _loyalty : value, _loyaltyMinValue, _loyaltyMaxValue);
        }

        public int LastBreakDay
        {
            get => _lastBreakDay;
            set => _lastBreakDay = Mathf.Max(0, value);
        }
        
        public BusyReason BusyReason
        {
            get => _busyReason;
            set => _busyReason = value;
        }
        
        public bool IsProductivityFrozen { get; set; }
        public bool IsLoyaltyFrozen { get; set; }
        
        // Методы.
        public bool IsBusy => _busyReason != BusyReason.None;
        
        // Больше не используется:
        
        [Obsolete("ChangeProductivity больше не используется -- используется свойство Productivity.")]
        public void ChangeProductivity(int delta)
        {
            if (_isProductivityFrozen)
                return;
        
            _productivity = Mathf.Clamp(_productivity + delta, _productivityMinValue, _productivityMaxValue);
        }

        [Obsolete("ChangeLoyalty больше не используется -- используется свойство Loyalty.")]
        public void ChangeLoyalty(int delta)
        {
            if (_isLoyaltyFrozen)
                return;
        
            _loyalty = Mathf.Clamp(_loyalty + delta, _loyaltyMinValue, _loyaltyMaxValue);
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