using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "IntrovertTraitDef", menuName = "Scriptable Objects/Traits/IntrovertTraitDef")]
    public class IntrovertTraitDef : TraitDef
    {
        public override int ModifySocial(int baseValue) => baseValue - 1;
        // If sent to task alone - +1 to every skill except social
        // if sent with someone - -1 to every skill except social
    }
}
