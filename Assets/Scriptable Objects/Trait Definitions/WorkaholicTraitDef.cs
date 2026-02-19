using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "WorkaholicTraitDef", menuName = "Scriptable Objects/Traits/WorkaholicTraitDef")]
    public class WorkaholicTraitDef : TraitDef
    {
        public override int ModifySocial(int baseValue) => baseValue - 1;

        public override int OnNoBreakPenalty(int baseLoyalty, int daysWithoutBreak)
        {
            if (daysWithoutBreak > 4)
            {
                int smallerPenalty = (daysWithoutBreak - 4) * 2;
                return baseLoyalty - smallerPenalty;
            }
            return baseLoyalty;
        }

        public override int OnStartOfDayProductivity(int baseProductivity, bool hadCoffeeToday) => baseProductivity + 10;
    }
}

