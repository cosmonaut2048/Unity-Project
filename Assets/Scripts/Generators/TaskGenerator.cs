using System.Collections.Generic;
using Content;
using UnityEngine;

namespace Generators
{
    public class TaskGenerator : MonoBehaviour
    {
        [Header("Generator Settings")]
        [SerializeField] private List<TaskDef> tasks;

        public TaskDef GetRandomTask()
        {
            return tasks[Random.Range(0, tasks.Count)];
        }
    }
}