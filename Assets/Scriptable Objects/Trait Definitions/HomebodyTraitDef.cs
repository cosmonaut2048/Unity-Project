using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "HomebodyTraitDef", menuName = "Scriptable Objects/Traits/HomebodyTraitDef")]
    public class HomebodyTraitDef : TraitDef
    {
        // Если отправить на задание, продуктивность и преданность падают.
        private readonly int _homebodyProductivityPenalty = 20;
        private readonly int _homebodyLoyaltyPenalty = 5;
        public override int OnTaskProductivity(int baseProductivity) => baseProductivity - _homebodyProductivityPenalty;
        public override int OnTaskLoyalty(int baseLoyalty) => baseLoyalty - _homebodyLoyaltyPenalty;
    }
}
