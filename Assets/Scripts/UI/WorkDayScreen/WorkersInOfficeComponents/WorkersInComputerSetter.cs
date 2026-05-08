using System.Collections.Generic;
using System.Linq;
using Runtime;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen.WorkersInOfficeComponents
{
    public class WorkersInComputerSetter
    {
        public void ClearAllCards(VisualElement workerCallCardContainer)
        {
            List<VisualElement> elements = workerCallCardContainer?.Children().ToList();
            
            if (elements == null) return;
            
            foreach (var element in elements)
            {
                if (element != null)
                {
                    workerCallCardContainer.Remove(element);
                }
            }
        }

        public void CreateAllCards(List<WorkerInComputer> workers, List<WorkerRuntime> workersRuntime, VisualElement workerCallCardContainer)
        {
            if (workersRuntime.Count == 0)
                return;
            
            foreach (var workerRuntime in workersRuntime)
            {
                workers.Add(new WorkerInComputer(workerRuntime));
            }

            foreach (var worker in workers)
            {
                worker.CreateCard(workerCallCardContainer);
            }
        }
    }
}