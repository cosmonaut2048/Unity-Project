using Content;
using Runtime;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "AdventurousTraitDef", menuName = "Scriptable Objects/Traits/AdventurousTraitDef")]
    [IncompatibleWith(typeof(HomebodyTraitDef))]  
    public class AdventurousTraitDef : TraitDef
    {
        // Когда на задании:
        // Продуктивность замораживается, а по окончании задания восстанавливается до 100 если была ниже.
        // Преданность замораживается, а по окончании задания восстанавливается до 100 если была ниже.
        public override bool OnTaskFreezeProductivity(WorkerRuntime worker)
        {
            return worker.BusyReason == BusyReason.OnTask;
        }

        public override bool OnTaskFreezeLoyalty(WorkerRuntime worker)
        {
            return worker.BusyReason == BusyReason.OnTask;
        }

        public override int OnTaskProductivity(int baseProductivity)
        {
            if (baseProductivity < 100)
                baseProductivity = 100;
            return baseProductivity;
        }

        public override int OnTaskLoyalty(int baseLoyalty)
        {
            if (baseLoyalty < 100)
                baseLoyalty = 100;
            return baseLoyalty;
        }
    }
}
