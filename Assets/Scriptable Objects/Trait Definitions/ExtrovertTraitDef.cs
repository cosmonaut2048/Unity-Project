using Content;
using Runtime;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "ExtrovertTraitDef", menuName = "Scriptable Objects/Traits/ExtrovertTraitDef")]
    [IncompatibleWith(typeof(IntrovertTraitDef))]
    public class ExtrovertTraitDef : TraitDef
    {
        // Если на задании один -- -1 ко всем навыкам кроме социального.
        // Если на задании НЕ один -- +1 ко всем навыкам кроме социального.
        public override int ModifySocial(int baseValue) => baseValue + 1; // Перманентно +1 к социальному навыку.
        
        public override int ModifyPatienceConditional(int baseValue, TaskRuntime task)
        {
            if (task.Workers.Count != 1)
                return baseValue + 1;
            
            return baseValue - 1;
        }

        public override int ModifyIntellectualConditional(int baseValue, TaskRuntime task)
        {
            if (task.Workers.Count != 1)
                return baseValue + 1;
            
            return baseValue - 1;
        }

        public override int ModifyPhysicalConditional(int baseValue, TaskRuntime task)
        {
            if (task.Workers.Count != 1)
                return baseValue + 1;
            
            return baseValue - 1;
        }
    }
}
