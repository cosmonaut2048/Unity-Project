using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class OfficeWorkerPlacement : MonoBehaviour
    {
        public static OfficeWorkerPlacement Instance { get; private set; }
        
        [SerializeField] private int hallCapacity = 3;
        [SerializeField] private int mainRoomCapacity = 3;
        [SerializeField] private int secondRoomCapacity = 5;
        [SerializeField] private int kitchenCapacity = 5;
        
        // Работники -- распределение по комнатам.
        [SerializeField] private List<WorkerRuntime> workersInHall = new List<WorkerRuntime>();
        [SerializeField] private List<WorkerRuntime> workersInMainRoom = new List<WorkerRuntime>();
        [SerializeField] private List<WorkerRuntime> workersInSecondRoom = new List<WorkerRuntime>();
        [SerializeField] private List<WorkerRuntime> workersInKitchen = new List<WorkerRuntime>();
        [SerializeField] private List<WorkerRuntime> workersInComputer = new List<WorkerRuntime>();
        
        public int HallCapacity => hallCapacity;
        public int MainRoomCapacity => mainRoomCapacity;
        public int SecondRoomCapacity => secondRoomCapacity;
        public int KitchenCapacity => kitchenCapacity;
        
        public List<WorkerRuntime> WorkersInHall => workersInHall;
        public List<WorkerRuntime> WorkersInMainRoom => workersInMainRoom;
        public List<WorkerRuntime> WorkersInSecondRoom => workersInSecondRoom;
        public List<WorkerRuntime> WorkersInKitchen => workersInKitchen;
        public List<WorkerRuntime> WorkersInComputer => workersInComputer;

        public void AddWorkerToHall(WorkerRuntime worker)
        {
            if (workersInHall.Count > hallCapacity)
                AddWorkerToComputer(worker);
            else
                workersInHall.Add(worker);
        }

        public void AddWorkerToMainRoom(WorkerRuntime worker)
        {
            if (workersInMainRoom.Count > mainRoomCapacity)
                AddWorkerToComputer(worker);
            else
                workersInMainRoom.Add(worker);
        }

        public void AddWorkerToSecondRoom(WorkerRuntime worker)
        {
            if (workersInSecondRoom.Count > secondRoomCapacity)
                AddWorkerToComputer(worker);
            else
                workersInSecondRoom.Add(worker);
        }

        public void AddWorkerToKitchen(WorkerRuntime worker)
        {
            if (workersInKitchen.Count > kitchenCapacity)
                AddWorkerToComputer(worker);
            else
                workersInKitchen.Add(worker);
        }

        public void AddWorkerToComputer(WorkerRuntime worker)
        {
            workersInComputer.Add(worker);
        }

        public void ClearAllRooms()
        {
            workersInHall.Clear();
            workersInMainRoom.Clear();
            workersInSecondRoom.Clear();
            workersInKitchen.Clear();
            workersInComputer.Clear();
        }
        
        public void SetWorkersInRooms(List<WorkerRuntime> workers)
        {
            foreach (var worker in workers)
            {
                int randRoom = Random.Range(0, 4);

                switch (randRoom)
                {
                    case 0:
                        OfficeWorkerPlacement.Instance.AddWorkerToHall(worker);
                        break;
                    case 1:
                        OfficeWorkerPlacement.Instance.AddWorkerToMainRoom(worker);
                        break;
                    case 2:
                        OfficeWorkerPlacement.Instance.AddWorkerToSecondRoom(worker);
                        break;
                    case 3:
                        OfficeWorkerPlacement.Instance.AddWorkerToKitchen(worker);
                        break;
                }
            }
        }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}