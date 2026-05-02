using System.Collections.Generic;
using Content;
using Core.TaskLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace Runtime
{
    public class OfficeRuntime : Office
    {
        public static OfficeRuntime Instance { get; private set; }
        
        [SerializeField] private List<ItemDef> inventory =  new List<ItemDef>();
        [SerializeField] private int dayOfTheWeek;
        // Работники.
        [SerializeField] private List<WorkerRuntime> hiredWorkers = new List<WorkerRuntime>();
        [SerializeField] private List<WorkerRuntime> firedWorkersToday =  new List<WorkerRuntime>();
        // Квота.
        [SerializeField] private QuotaRuntime currentQuota;
        // Задание.
        [SerializeField] private TaskDef availableTask;
        [CanBeNull] [SerializeField] private TaskRuntime currentTask;

        [CanBeNull] [SerializeField] private TotalTaskResult lastTaskResult;
        // Кофе.
        [SerializeField] private int coffee;
        [SerializeField] private int coffeeConsumedToday;
        [SerializeField] private int coffeeObtainedToday;
        // Перерывы.
        [SerializeField] private int breakVouchers;
        [SerializeField] private int breakVouchersUsedToday;
        // Отчёт.
        [SerializeField] private DailyReport dailyReport;
        
        public List<ItemDef> Inventory => inventory;
        public int DayOfTheWeek => dayOfTheWeek;
        public List<WorkerRuntime> HiredWorkers => hiredWorkers;
        public List<WorkerRuntime> FiredWorkersToday => firedWorkersToday;
        public int Coffee => coffee;
        public int CoffeeConsumedToday => coffeeConsumedToday;
        public int CoffeeObtainedToday => coffeeObtainedToday;
        public int BreakVouchers => breakVouchers;
        public int BreakVouchersUsedToday => breakVouchersUsedToday;
        public QuotaRuntime CurrentQuota => currentQuota;
        public TaskDef AvailableTask => availableTask;
        public TaskRuntime CurrentTask => currentTask;
        public TotalTaskResult LastTaskResult => lastTaskResult;
        public DailyReport DailyReport => dailyReport;

        public void ClearRuntimeData()
        {
            inventory?.Clear();
            hiredWorkers?.Clear();
        }
        
        public void SetCoffee(int coffeeAmount) { coffee = coffeeAmount; }
        public void SetBreakVouchers(int voucherAmount) { breakVouchers = voucherAmount; }

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
        
        public void TickCoffee()
        {
            if (coffee > 0)
                coffee--;
        }

        public void TickBreakVouchers()
        {
            if (breakVouchers > 0)
                breakVouchers--;
        }
        
        public void AddWorkers(List<WorkerRuntime> workers)
        {
            foreach (WorkerRuntime worker in workers)
                hiredWorkers?.Add(worker);
        }

        public List<WorkerRuntime> WorkersInOffice()
        {
            List<WorkerRuntime> workersInOffice = new List<WorkerRuntime>();

            foreach (WorkerRuntime worker in hiredWorkers)
            {
                if (worker.BusyReason == BusyReason.None)
                    workersInOffice.Add(worker);
            }
            
            return workersInOffice;
        }

        public List<WorkerRuntime> BusyWorkers()
        {
            List<WorkerRuntime> busyWorkers = new List<WorkerRuntime>();

            foreach (WorkerRuntime worker in hiredWorkers)
            {
                if (worker.BusyReason != BusyReason.None)
                    busyWorkers.Add(worker);
            }
            
            return busyWorkers;
        }

        public void SetAvailableTask(TaskDef newTask)
        {
            availableTask = newTask;
        }

        public void SetCurrentTask(TaskRuntime newTask)
        {
            currentTask = newTask;
        }

        public void SetDailyReport(DailyReport newReport)
        {
            dailyReport = newReport;
        }

        public void SetActiveTask(TaskDef assignedTask, List<WorkerRuntime> assignedWorkers)
        {
            if (currentTask && !currentTask.IsFinished) return;
            if (assignedWorkers.Count < assignedTask.WorkerAmountRequired) return;
            
            currentTask = ScriptableObject.CreateInstance<TaskRuntime>();
            currentTask?.InitializeTaskRuntime(assignedTask, assignedWorkers);

            foreach (var worker in assignedWorkers)
            {
                if (hiredWorkers.Contains(worker))
                    worker.SetBusyReason = BusyReason.OnTask;
            }
        }

        public void FreeActiveTask()
        {
            if (!currentTask) return;
            
            foreach (var worker in currentTask.Workers)
            {
                if (hiredWorkers.Contains(worker))
                    worker.SetBusyReason = BusyReason.None;
            }
            
            currentTask = null;
        }

        public void SetTaskResult(TotalTaskResult totalTaskResult)
        {
            lastTaskResult = totalTaskResult;
        }

        public void ClearFiredWorkersToday()
        {
            firedWorkersToday.Clear();
        }

        public void FireWorker(WorkerRuntime worker)
        {
            firedWorkersToday.Add(worker);
            hiredWorkers.Remove(worker);
        }

        public void TickDayOfTheWeek()
        {
            dayOfTheWeek++;
        }
    }
}
