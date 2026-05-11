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
            
            // Очищаем состояние офиса.
            OfficeRuntime.Instance.SetCoffeeConsumedToday(0);
            OfficeRuntime.Instance.SetCoffeeObtainedToday(0);
            OfficeRuntime.Instance.SetBreakVouchersUsedToday(0);
            
            // Обновляем состояние квоты.
            dayStartCalculator.OnDayStartQuota();
            
            // Обновляем состояние заданий.
            dayStartCalculator.OnDayStartTask();
            OfficeRuntime.Instance.SetAvailableTask(taskGenerator.GetRandomTask());
            
            // Расставляем работников по комнатам в офисе.
            OfficeWorkerPlacement.Instance.ClearAllRooms();
            OfficeWorkerPlacement.Instance.SetWorkersInRooms(OfficeRuntime.Instance.WorkersInOffice());
            
            // Обновляем счётчик дня.
            OfficeRuntime.Instance.TickDayOfTheWeek();
        }
    }
}