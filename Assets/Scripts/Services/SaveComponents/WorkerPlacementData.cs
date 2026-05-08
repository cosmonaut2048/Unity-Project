using System.Collections.Generic;
using Runtime;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class WorkerPlacementData
    {
        public List<WorkerRuntimeData> workersInHall = new List<WorkerRuntimeData>();
        public List<WorkerRuntimeData> workersInMainRoom = new List<WorkerRuntimeData>();
        public List<WorkerRuntimeData> workersInSecondRoom = new List<WorkerRuntimeData>();
        public List<WorkerRuntimeData> workersInKitchen = new List<WorkerRuntimeData>();
        public List<WorkerRuntimeData> workersInComputer = new List<WorkerRuntimeData>();
        
        public void SetDataFromPlacement()
        {
            workersInHall = GetWorkerRuntimeDataList(OfficeWorkerPlacement.Instance.WorkersInHall);
            workersInMainRoom = GetWorkerRuntimeDataList(OfficeWorkerPlacement.Instance.WorkersInMainRoom);
            workersInSecondRoom = GetWorkerRuntimeDataList(OfficeWorkerPlacement.Instance.WorkersInSecondRoom);
            workersInKitchen = GetWorkerRuntimeDataList(OfficeWorkerPlacement.Instance.WorkersInKitchen);
            workersInComputer = GetWorkerRuntimeDataList(OfficeWorkerPlacement.Instance.WorkersInComputer);
        }
        
        private List<WorkerRuntimeData> GetWorkerRuntimeDataList(List<WorkerRuntime> workers)
        {
            List<WorkerRuntimeData> workerDataList = new List<WorkerRuntimeData>();
            
            foreach (var worker in workers)
            {
                WorkerRuntimeData workerData = new WorkerRuntimeData();
                workerData.SetDataFromWorkerRuntime(worker);
                workerDataList.Add(workerData);
            }
            
            return workerDataList;
        }
    }
}