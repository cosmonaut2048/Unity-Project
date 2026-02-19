using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "ExtrovertTraitDef", menuName = "Scriptable Objects/Traits/ExtrovertTraitDef")]
    public class ExtrovertTraitDef : TraitDef
    {
        public override int ModifySocial(int baseValue) => baseValue + 1;
        
        // if sent to task with someone - +1 to every skill except social
        // in sent alone - -1 to every skill except social
    }
}
