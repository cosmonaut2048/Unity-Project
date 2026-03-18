using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "WorkaholicTraitDef", menuName = "Scriptable Objects/Traits/WorkaholicTraitDef")]
    public class WorkaholicTraitDef : TraitDef
    {
        // -1 к социальным навыкам.
        public override int ModifySocial(int baseValue) => baseValue - 1;

        // Медленнее устаёт без перекуров.
        public override int OnNoBreakLoyaltyPenalty(int baseLoyalty, int daysWithoutBreak)
        {
            if (daysWithoutBreak > 4)
            {
                int smallerPenalty = (daysWithoutBreak - 4) * 2;
                return baseLoyalty - smallerPenalty;
            }
            return baseLoyalty;
        }

        // Медленнее теряет продуктивность (+10 компенсирует базовые потери).
        public override int OnStartOfDayProductivity(int baseProductivity, bool hadCoffeeToday) => baseProductivity + 10;
    }
}

