using Content;
using Runtime;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "PerfectionistTraitDef", menuName = "Scriptable Objects/Traits/PerfectionistTraitDef")]
    public class PerfectionistTraitDef : TraitDef
    {
        // Теряет меньше продуктивности, но теряет больше преданности.
        public override  bool IsUniqueLoyaltyTick() => true;
        public override int LoyaltyTickSize(WorkerRuntime worker) => worker.Worker.BaseLoyaltyTickSize * 2;
        public override bool IsUniqueProductivityTick() => true;
        public override int ProductivityTickSize(WorkerRuntime worker) => worker.Worker.BaseProductivityTickSize / 2;
    }
}
