using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "SicklyTraitDef", menuName = "Scriptable Objects/Traits/SicklyTraitDef")]
    public class SicklyTraitDef : TraitDef
    {
        // Рандомно может пропустить рабочий день.
        public override void OnDayStart(WorkerDef workerDef)
        {
            Debug.Log("Trait Not Implemented");   
        }
    }
}
