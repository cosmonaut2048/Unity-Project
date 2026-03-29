using Content;
using Runtime;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "WorkaholicTraitDef", menuName = "Scriptable Objects/Traits/WorkaholicTraitDef")]
    [IncompatibleWith(typeof(SlackerTraitDefinition))]
    public class WorkaholicTraitDef : TraitDef
    {
        private readonly int _thresholdBonus = 2;
        // -1 к социальным навыкам.
        public override int ModifySocial(int baseValue) => baseValue - 1;

        // Порог перерывов повышен.
        public override bool IsUniqueLoyaltyTick() => true;
        public override int LoyaltyTickSize(WorkerRuntime worker)
        {
            if (worker.LastBreakDay < worker.BaseNoBreakThreshold + _thresholdBonus)
                return 0;
            return worker.BaseLoyaltyTickSize;
        }

        // Теряет вдвое меньше продуктивности.
        public override bool IsUniqueProductivityTick() => true;
        public override int ProductivityTickSize(WorkerRuntime worker) => worker.BaseProductivityTickSize / 2;
    }
}

