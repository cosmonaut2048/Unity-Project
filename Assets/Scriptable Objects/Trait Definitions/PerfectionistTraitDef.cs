using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "PerfectionistTraitDef", menuName = "Scriptable Objects/Traits/PerfectionistTraitDef")]
    public class PerfectionistTraitDef : TraitDef
    {
        // Работает лучше, быстрее устаёт.
        private readonly int _perfectionistBonus = 20;
        
        public override int OnStartOfDayProductivity(int baseProductivity, bool hadCoffeeToday)
        {
            return _perfectionistBonus + baseProductivity;
        }
        
        public override int OnNoBreakLoyaltyPenalty(int baseLoyalty, int daysWithoutBreak)
        {
            if (daysWithoutBreak > 2)
            {
                int extraPenalty = (daysWithoutBreak - 2) * 10;
                return baseLoyalty - extraPenalty;
            }
            return baseLoyalty;
        }
    }
}
