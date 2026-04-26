using System.Collections.Generic;
using System.Linq;
using Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class WorkerSetter
    {
        public List<VisualElement> CacheWorkers(VisualElement workersContainer)
        {
            return workersContainer?.Children().ToList() ?? new List<VisualElement>();
        }

        public void SetWorker(VisualElement workerElement, WorkerRuntime worker)
        {
            workerElement.style.display = DisplayStyle.Flex;
            workerElement.style.backgroundImage = new StyleBackground(worker.Worker.Appearance.FullBodySprite);
        }

        public void SetAllWorkers(List<VisualElement> workerElements, List<WorkerRuntime> workers, int roomCapacity)
        {
            if (workers.Count > roomCapacity)
            {
                Debug.Log($"WorkerSetter.SetAllWorkers(): workers.Count = {workers.Count}, roomCapacity = {roomCapacity}");
                return;
            }

            for (int i = 0; i < workers.Count; i++)
            {
                SetWorker(workerElements[i], workers[i]);
            }
        }

        public void HideWorkers(List<VisualElement> workers)
        {
            if (workers == null) return;
            
            foreach (var worker in workers)
            {
                if (worker != null)
                {
                    worker.style.display = DisplayStyle.None;
                }
            }
        }
    }
}