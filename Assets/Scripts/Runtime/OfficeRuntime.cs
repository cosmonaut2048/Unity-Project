using System.Collections.Generic;
using Content;
using UnityEngine;

namespace Runtime
{
    public class OfficeRuntime : Office
    {
        public static OfficeRuntime Instance { get; private set; }
        
        [SerializeField] private List<ItemDef> inventory =  new List<ItemDef>();
        [SerializeField] private List<WorkerRuntime> hiredWorkers = new List<WorkerRuntime>();
        [SerializeField] private int coffee;
        [SerializeField] private int breakVouchers;
        
        public List<ItemDef> Inventory => inventory;
        public List<WorkerRuntime> HiredWorkers => hiredWorkers;
        public int Coffee => coffee;
        public int BreakVouchers => breakVouchers;

        public void ClearRuntimeData()
        {
            inventory?.Clear();
            hiredWorkers?.Clear();
        }
        
        public void SetCoffee(int coffeeAmount) { coffee = coffeeAmount; }
        public void SetBreakVouchers(int voucherAmount) { breakVouchers = voucherAmount; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void TickCoffee()
        {
            if (coffee > 0)
                coffee--;
        }

        public void TickBreakVouchers()
        {
            if (breakVouchers > 0)
                breakVouchers--;
        }
        
        public void AddWorkers(List<WorkerRuntime> workers)
        {
            foreach (WorkerRuntime worker in workers)
                hiredWorkers?.Add(worker);
        }

        public void FireWorkers(List<WorkerRuntime> workers)
        {
            foreach (WorkerRuntime worker in workers)
                hiredWorkers?.Remove(worker);
        }
    }
}
