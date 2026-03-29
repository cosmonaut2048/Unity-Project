using Content;
using Runtime;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "SicklyTraitDef", menuName = "Scriptable Objects/Traits/SicklyTraitDef")]
    public class SicklyTraitDef : TraitDef
    {
        // Случайно может пропустить рабочий день по болезни, если не находится на задании.
        private readonly int _sickRate = 5;
        public override void OnStartOfDay(WorkerRuntime workerDef)
        {
            if (workerDef.BusyReason == BusyReason.OnTask) return;
            
            workerDef.BusyReason = BusyReason.None;
                
            if (Random.Range(1, _sickRate + 1) == _sickRate)
                workerDef.BusyReason = BusyReason.Sick;
        }
    }
}
