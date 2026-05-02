using Content;
using Runtime;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "SlackerTraitDef", menuName = "Scriptable Objects/Traits/SlackerTraitDef")]
    [IncompatibleWith(typeof(WorkaholicTraitDef))]
    public class SlackerTraitDefinition : TraitDef
    {
        // +1 к социальным навыкам.
        public override int ModifySocial(int baseValue) => baseValue + 1;

        // Сильнее устаёт без перерывов.
        public override bool IsUniqueLoyaltyTick() => true;
        public override int LoyaltyTickSize(WorkerRuntime worker) => worker.Worker.BaseLoyaltyTickSize * 2;

        // Теряет продуктивность пропорционально количеству дней после порога без перерывов.
        public override bool IsUniqueProductivityTick() => true; 

        public override int ProductivityTickSize(WorkerRuntime worker)
        {
            return (worker.LastBreakDay - worker.Worker.BaseNoBreakThreshold) * worker.Worker.BaseProductivityTickSize;;
        }
        
        // Перерыв восполняет продуктивность до 100, если она была ниже.
        public override void OnBreak(WorkerRuntime worker)
        {
            if (worker.Loyalty < 100)
                worker.SetLoyalty(100);
        }
    }
}