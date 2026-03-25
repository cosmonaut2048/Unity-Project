using Content;
using Runtime;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "CoffeeAddictTraitDef", menuName = "Scriptable Objects/Traits/CoffeeAddictTraitDef")]
    public class CoffeeAddictTraitDef : TraitDef
    {
        // Если выпил кофе:
        // Работает на максимальной продуктивности 3 дня.
        // Когда счётчик обнуляется, продуктивность падает до 50.
        
        // Пока счётчик не достиг 0, обновить состояние нельзя.

        // private int _caffeineRushCounter;
        // private readonly int _caffeineRushCounterFresh = 3;
        
        // private readonly int _caffeineRushProductivity = 200;
        // private readonly int _noCaffeineRushProductivity = 200;
        
        // public override void OnCoffee(WorkerRuntime workerRuntime)
        // {
        //     if (_caffeineRushCounter == 0)
        //         _caffeineRushCounter = _caffeineRushCounterFresh;
        // }

        // public override void OnDayStart(WorkerRuntime workerRuntime)
        // {
        //     if (_caffeineRushCounter > 0)
        //         _caffeineRushCounter--;
        // }

        // public override int OnCoffeeProductivity(int baseProductivity)
        // {
        //     if (_caffeineRushCounter != 0)
        //         return _caffeineRushProductivity;
        //     
        //     return _noCaffeineRushProductivity;
        // }
    }
}
