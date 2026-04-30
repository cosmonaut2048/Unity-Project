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

            // Обновляем состояние работников.
            foreach (var worker in OfficeRuntime.Instance.HiredWorkers)
            {
                if (dayStartCalculator.IsLeavingCompany(worker))
                {
                    worker.FireWorker();
                    OfficeRuntime.Instance.FiredWorkersToday.Add(worker);
                    OfficeRuntime.Instance.HiredWorkers.Remove(worker);
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