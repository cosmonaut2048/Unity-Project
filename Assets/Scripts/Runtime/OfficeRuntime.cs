using System.Collections.Generic;
using Content;
using JetBrains.Annotations;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "OfficeRuntime", menuName = "Scripts/Runtime/OfficeRuntime")]
    public class OfficeRuntime : Office
    {
        [CanBeNull] [SerializeField] private List<ItemDef> inventory = new List<ItemDef>();
        [CanBeNull] [SerializeField] private List<WorkerDef> hiredWorkers = new List<WorkerDef>();
        [SerializeField] private int coffee;
        [SerializeField] private int breakVouchers;
        
        public List<ItemDef> Inventory => inventory;
        public List<WorkerDef> HiredWorkers => hiredWorkers;
        public int Coffee => coffee;
        public int BreakVouchers => breakVouchers;

        public void ClearRuntimeData()
        {
            if (inventory != null) inventory.Clear();
            if (hiredWorkers != null) hiredWorkers.Clear();
        }
    }
}