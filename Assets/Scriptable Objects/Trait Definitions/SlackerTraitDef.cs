using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "SlackerTraitDef", menuName = "Scriptable Objects/Traits/SlackerTraitDef")]
    public class SlackerTraitDefinition : TraitDef
    {
        public override int ModifySocial(int baseValue) => baseValue + 1;

        public override int OnNoBreakPenalty(int baseLoyalty, int daysWithoutBreak)
        {
            if (daysWithoutBreak > 2)
            {
                int extraPenalty = (daysWithoutBreak - 2) * 10;
                return baseLoyalty - extraPenalty;
            }
            return baseLoyalty;
        }

        public override int OnStartOfDayProductivity(int baseProductivity, bool hadCoffeeToday) => baseProductivity - 10;
    }
}