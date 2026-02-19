using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "AdventurousTraitDef", menuName = "Scriptable Objects/Traits/AdventurousTraitDef")]
    public class AdventurousTraitDef : TraitDef
    {
        // When sent to task, loyalty is unaffected
        // productivity is either unaffected or restored to 100 (if it was lower previously)
    }
}
