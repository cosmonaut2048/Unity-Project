using System.Collections.Generic;
using Content;
using UnityEngine;

namespace Scriptable_Objects.Catalogs
{
    [CreateAssetMenu(fileName = "TaskCatalog", menuName = "Scriptable Objects/Catalogs/TaskCatalog")]
    public class TaskCatalog : ScriptableObject
    {
        [SerializeField] private List<TaskDef> allTasks;
        
        // Доступ по названию.
        private Dictionary<string, TaskDef> _tasksByName;
        // Readonly property.
        public IReadOnlyList<TaskDef> AllTasks => allTasks;

        private void OnEnable()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            _tasksByName = new Dictionary<string, TaskDef>();
            foreach (var task in allTasks)
            {
                if (task != null)
                    _tasksByName.TryAdd(task.TaskName, task);
            }
        }

        public TaskDef GetTaskByName(string taskName)
        {
            if (_tasksByName == null)
                InitializeDictionary();

            return _tasksByName.GetValueOrDefault(taskName);
        }
        
        #if UNITY_EDITOR
        public void RefreshCatalog()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:task ");
            allTasks = new List<TaskDef>();

            foreach (var guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var task = UnityEditor.AssetDatabase.LoadAssetAtPath<TaskDef>(path);
                
                if (task)
                    allTasks.Add(task);
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }
}
