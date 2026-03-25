using System.Collections.Generic;
using Content;
using JetBrains.Annotations;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "OfficeRuntime", menuName = "Scripts/Runtime/OfficeRuntime")]
    public class OfficeRuntime : Office
    {
        [CanBeNull] public List<ItemDef> inventory = new List<ItemDef>();
        [CanBeNull]public List<WorkerDef> hiredWorkers = new List<WorkerDef>();

        public void ClearRuntimeData()
        {
            if (inventory != null) inventory.Clear();
            if (hiredWorkers != null) hiredWorkers.Clear();
        }
    }
}