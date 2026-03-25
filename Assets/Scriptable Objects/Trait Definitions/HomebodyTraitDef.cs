using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "HomebodyTraitDef", menuName = "Scriptable Objects/Traits/HomebodyTraitDef")]
    public class HomebodyTraitDef : TraitDef
    {
        // Если отправить на задание, продуктивность и преданность падают.
        
        // Когда на задании:
        // Продуктивность понижается до 100, если была выше.
        // Преданность понижается до 100, если была выше.
        private readonly int _homebodyProductivityPenalty = 20;
        private readonly int _homebodyLoyaltyPenalty = 5;
        
        public override int OnTaskProductivity(int baseProductivity)
        {
            baseProductivity -= _homebodyProductivityPenalty;
            
            if (baseProductivity > 100)
                baseProductivity = 100;
            
            return baseProductivity;
        }

        public override int OnTaskLoyalty(int baseLoyalty)
        {
            baseLoyalty -= _homebodyLoyaltyPenalty;
            
            if (baseLoyalty > 100)
                baseLoyalty = 100;
            
            return baseLoyalty;
        }
    }
}
