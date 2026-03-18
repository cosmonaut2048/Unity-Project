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
                if (worker != null)
                    _workersByName.TryAdd(worker.workerName, worker);
            }
        }

        public WorkerDef GetWorkerByName(string workerName)
        {
            if (_workersByName == null)
                InitializeDictionary();
            
            return _workersByName.GetValueOrDefault(workerName);
            
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
    
        private WorkerDef CreateDefaultWorker()
        {
            return ScriptableObject.CreateInstance<WorkerDef>();
        }
        
        // private void ShuffleList<T>(List<T> list)
        // {
        //     for (int i = 0; i < list.Count; i++)
        //     {
        //         int randomIndex = Random.Range(i, list.Count);
        //         (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        //     }
        // }
        
    }
}
