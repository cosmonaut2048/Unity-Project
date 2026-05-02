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

        private int _caffeineRushCountdown;
        private readonly int _caffeineRushCountdownFresh = 3;
        
        private readonly int _caffeineRushProductivity = 200;
        private readonly int _noCaffeineRushProductivity = 50;
        
        public override void OnCoffee(WorkerRuntime workerRuntime)
        {
            if (_caffeineRushCountdown == 0)
                _caffeineRushCountdown = _caffeineRushCountdownFresh;
            
            if (_caffeineRushCountdown > 0)
                workerRuntime.SetProductivity(_caffeineRushProductivity);
            else
                workerRuntime.SetProductivity(_noCaffeineRushProductivity);
        }

        public override void OnStartOfDay(WorkerRuntime workerRuntime)
        {
            if (_caffeineRushCountdown > 0)
                _caffeineRushCountdown--;
        }
    }
}
