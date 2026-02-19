using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "SicklyTraitDef", menuName = "Scriptable Objects/Traits/SicklyTraitDef")]
    public class SicklyTraitDef : TraitDef
    {
        // Can randomly miss work
        public override void OnDayStart(Worker worker)
        {
            Debug.Log("Trait Not Implemented");   
        }
    }
}
