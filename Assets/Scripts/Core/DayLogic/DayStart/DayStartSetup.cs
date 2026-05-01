using Core.DayLogic.TickCalculation;
using Generators;
using Runtime;
using UnityEngine;

namespace Core.DayLogic.DayStart
{
    public class DayStartSetup : MonoBehaviour
    {
        [SerializeField] private TaskGenerator taskGenerator;
        
        public void SetupDayStart()
        {
            ProductivityTickCalculator productivityCalculator = new ProductivityTickCalculator();
            DayStartCalculator dayStartCalculator = new DayStartCalculator();
            
            // Очищаем список с предыдущего дня.
            OfficeRuntime.Instance.ClearFiredWorkersToday();
            
            // Обновляем состояние работников.
            for (int i = OfficeRuntime.Instance.HiredWorkers.Count - 1; i >= 0; i--)
            {
                var worker = OfficeRuntime.Instance.HiredWorkers[i];
                if (dayStartCalculator.IsLeavingCompany(worker))
                {
                    worker.FireWorker();
                    OfficeRuntime.Instance.FireWorker(worker);
                }
                else
                {
                    dayStartCalculator.DayStartWorker(worker);
                    dayStartCalculator.DayStartWorkerProductivity(worker, productivityCalculator);
                }
            }
            
            
            // Обновляем состояние заданий.
            dayStartCalculator.OnDayStartTask();
            OfficeRuntime.Instance.SetAvailableTask(taskGenerator.GetRandomTask());
            
            // Расставляем работников по комнатам в офисе.
            OfficeWorkerPlacement.Instance.ClearAllRooms();
            OfficeWorkerPlacement.Instance.SetWorkersInRooms(OfficeRuntime.Instance.WorkersInOffice());
        }
    }
}