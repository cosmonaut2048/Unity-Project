using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "LongCommuteTraitDef", menuName = "Scriptable Objects/Traits/LongCommuteTraitDef")]
    public class LongCommuteTraitDef : TraitDef
    {
        // Долго добирается до работы: из-за этого продуктивность хуже (в пределах случайности).
        private int _commuteLength;
        
        public override int OnStartOfDayProductivity(int baseProductivity, bool hadCoffeeToday)
        {
            _commuteLength = Random.Range(1, 6);
            return baseProductivity - _commuteLength * 10;
        }
    }
}
