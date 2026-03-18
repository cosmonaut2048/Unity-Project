using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "NeuroticTraitDef", menuName = "Scriptable Objects/Traits/NeuroticTraitDef")]
    public class NeuroticTraitDef : TraitDef
    {
        // Нестабильная продуктивность (может работать очень плохо или очень хорошо).
        private readonly int _productivityRandMin = -5;
        private readonly int _productivityRandMax = 6;
        
        public override int OnStartOfDayProductivity(int baseProductivity, bool hadCoffeeToday)
        {
            return baseProductivity - Random.Range(_productivityRandMin, _productivityRandMax) * 10;
        }
    }
}
