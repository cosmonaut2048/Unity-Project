using System.Collections.Generic;
using System.Linq;
using Content;
using UnityEngine;

namespace Scriptable_Objects.Catalogs
{
    [CreateAssetMenu(fileName = "WorkerCatalog", menuName = "Scriptable Objects/Catalogs/WorkerCatalog")]
    public class WorkerCatalog : ScriptableObject
    {
        [SerializeField] private List<WorkerDef> allWorkers;
        
        // Доступ по имени.
        private Dictionary<string, WorkerDef> _workersByName;
    
        // Readonly property
        public IReadOnlyList<WorkerDef> AllWorkers => allWorkers;
        
        private void OnEnable()
        {
            InitializeDictionary();
        }
        
        private void InitializeDictionary()
        {
            _workersByName = new Dictionary<string, WorkerDef>();
            foreach (var worker in allWorkers)
            {
                if (worker)
                    _workersByName.TryAdd(worker.Appearance.WorkerName, worker);
            }
        }
    
        public WorkerDef GetRandomWorker()
        {
            if (allWorkers.Count == 0)
            {
                Debug.LogWarning("No workers found.");
                return null;
            }
        
            return allWorkers[Random.Range(0, allWorkers.Count)];
        }
        
    }
}
