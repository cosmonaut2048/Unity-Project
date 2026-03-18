using UnityEngine;
using Content;

namespace Runtime
{
    [CreateAssetMenu(fileName = "Worker", menuName = "Scriptable Objects/Runtime/Worker")]
    public class WorkerRuntime : WorkerDef
    {
        private int _productivity;
        private int _loyalty;
        
        [Header("Worker State")]
        private bool _isEmployed;
        private bool _isBusy;
        private bool _hadCoffeeToday;
        private int _daysWithoutBreak;
        private int _lastBreakDay;
    }
}