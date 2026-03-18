using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "OfficeInventory", menuName = "Scriptable Objects/Content/OfficeInventory")]
    public class Office : ScriptableObject
    {
        [CanBeNull] public List<OfficeItem> inventory = new List<OfficeItem>();
        [CanBeNull]public List<WorkerDef> hiredWorkers = new List<WorkerDef>();
    }
}
