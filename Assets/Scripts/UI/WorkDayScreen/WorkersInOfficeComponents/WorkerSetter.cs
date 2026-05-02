using System.Collections.Generic;
using System.Linq;
using Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen.WorkersInOfficeComponents
{
    public class WorkerSetter
    {
        public List<WorkerInOffice> CreateWorkersInOffice(List<WorkerRuntime> workersRuntime, VisualElement allWorkersContainer)
        {
            List<WorkerInOffice> workers = new List<WorkerInOffice>();
            
            List<VisualElement> workerContainers = allWorkersContainer.Children().Where(x => x.ClassListContains("worker--container")).ToList();

            for (int i = 0; i < workerContainers.Count; i++)
            {
                if (i > workersRuntime.Count - 1) break;

                VisualElement giveContainer = workerContainers[i].Children()
                    .FirstOrDefault(x => x.ClassListContains("give--container"));
                    
                workers.Add(new WorkerInOffice
                (
                    workersRuntime[i],
                    workerContainers[i],
                    workerContainers[i].Children().FirstOrDefault(x => x.ClassListContains("worker--full")),
                    giveContainer?.Children().FirstOrDefault(x => x.ClassListContains("give--coffee")),
                    giveContainer?.Children().FirstOrDefault(x => x.ClassListContains("give--voucher"))
                    
                ));
            }
            
            return workers;
        }

        public void SetAllWorkers(List<WorkerInOffice> workers, int roomCapacity)
        {
            if (workers.Count > roomCapacity)
            {
                Debug.Log($"WorkerSetter.SetAllWorkers(): workers.Count = {workers.Count}, roomCapacity = {roomCapacity}");
                return;
            }
            foreach (var worker in workers)
            {
                worker.SetWorker();
            }
        }
        
        public void HideAllWorkers(VisualElement allWorkersContainer)
        {
            List<VisualElement> workerContainers = allWorkersContainer.Children().Where(x => x.ClassListContains("worker--container")).ToList();
            
            foreach (var container in workerContainers)
            {
                container.style.display = DisplayStyle.None;
            }
        }
    }
}