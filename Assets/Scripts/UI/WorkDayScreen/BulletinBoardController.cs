using System.Collections.Generic;
using System.Linq;
using Content;
using Gameflow;
using Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class BulletinBoardController : MonoBehaviour
    {
        private TaskDef _task;
        
        public TaskDef Task
        {
            get => _task;
            set
            {
                if (_task != value)
                {
                    _task = value;
                    OnTaskChanged();
                }
            }
        }
        
        // Текстовая информация.
        private Label _taskName;
        private Label _taskDescription;
        private Label _workersRequired;
        private Label _daysRequired;
        
        // Контейнеры навыков.
        private VisualElement _skillPatienceCellsContainer;
        private VisualElement _skillSocialCellsContainer;
        private VisualElement _skillIntellectualCellsContainer;
        private VisualElement _skillPhysicalCellsContainer;
        
        // Ячейки навыков (получаем из контейнеров).
        private List<VisualElement> _patienceCells;
        private List<VisualElement> _socialCells;
        private List<VisualElement> _intellectualCells;
        private List<VisualElement> _physicalCells;
        
        // Элементы записок.
        private VisualElement _stickerNotes;
        private VisualElement _workersContainer;
        private VisualElement _coffeeContainer;
        private VisualElement _vouchersContainer;
        private VisualElement _progressBar;
        private Label _progressBarText;
        private Label _noteText;
        
        // Кнопки.
        private VisualElement _backButton;

        void Start()
        {
            _task = OfficeRuntime.Instance.AvailableTask;
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Текстовая информация.
            _taskName = root.Q<Label>("task_name");
            _taskDescription = root.Q<Label>("task_description");
            _workersRequired = root.Q<Label>("workers_required");
            _daysRequired = root.Q<Label>("days_required");
            // Контейнеры навыков.
            _skillPatienceCellsContainer = root.Q<VisualElement>("Skill_Patience_Cells_Container");
            _skillSocialCellsContainer = root.Q<VisualElement>("Skill_Social_Cells_Container");
            _skillIntellectualCellsContainer = root.Q<VisualElement>("Skill_Intellectual_Cells_Container");
            _skillPhysicalCellsContainer = root.Q<VisualElement>("Skill_Physical_Cells_Container");
            // Элементы записок.
            _stickerNotes = root.Q<VisualElement>("Sticker_Notes");
            _workersContainer = root.Q<VisualElement>("Workers_Container");
            _coffeeContainer = root.Q<VisualElement>("Coffee_Container");
            _vouchersContainer = root.Q<VisualElement>("Vouchers_Container");
            _progressBar = root.Q<VisualElement>("progress_bar");
            _progressBarText = root.Q<Label>("progress_bar_text");
            _noteText = root.Q<Label>("note_text");
            // Кнопки.
            _backButton = root.Q<VisualElement>("back_button");
            
            // Кэшируем ячейки навыков.
            CacheSkillCells();
            
            // Отображаем данные на доску.
            OnTaskChanged();
            SetQuotaSticker();
            SetWorkersSticker();
            SetCoffeeSticker();
            SetVoucherSticker();
            if (OfficeRuntime.Instance.BusyWorkers().Count > 0)
            {
                _stickerNotes.style.display = DisplayStyle.Flex;
                SetNoteSticker();
            }
            else
            {
                _stickerNotes.style.display = DisplayStyle.None;
            }
            
            // Подписываемся на события.
            _backButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.HallsScene)));
        }

        private void SetNoteSticker()
        {
            List<string> busyReasonNotes = new List<string>();

            foreach (WorkerRuntime worker in OfficeRuntime.Instance.BusyWorkers())
            {
                switch (worker.BusyReason)
                {
                    case (BusyReason.Sick):
                        busyReasonNotes.Add($"{worker.Worker.Appearance.WorkerName} is sick today.");
                        break;
                    case (BusyReason.OnTask):
                        busyReasonNotes.Add($"{worker.Worker.Appearance.WorkerName} is on a task today.");
                        break;
                }
            }

            foreach (string note in busyReasonNotes)
            {
                _noteText.text += $"{note}\n"; 
            }
        }

        private void SetQuotaSticker()
        {
            _progressBar.style.width = 
                OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaSize != 0 ? 
                Length.Percent((float)OfficeRuntime.Instance.CurrentQuota.QuotaProgressNew / OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaSize * 100) 
                : Length.Percent(0);

            _progressBarText.text = 
                $"{OfficeRuntime.Instance.CurrentQuota.QuotaProgressNew}/{OfficeRuntime.Instance.CurrentQuota.StaticQuota.QuotaSize}";
        }

        private void SetCoffeeSticker()
        {
            if (OfficeRuntime.Instance.Coffee == 0)
            {
                AddNone(_coffeeContainer);
            }
            else
            {
                for (int i = 0; i < OfficeRuntime.Instance.Coffee; i++)
                {
                    AddObjectWithStyle(_coffeeContainer, "coffee--full");
                }
            }
        }

        private void SetVoucherSticker()
        {
            if (OfficeRuntime.Instance.BreakVouchers == 0)
            {
                AddNone(_vouchersContainer);
            }
            else
            {
                for (int i = 0; i < OfficeRuntime.Instance.BreakVouchers; i++)
                {
                    AddObjectWithStyle(_vouchersContainer, "voucher");
                }
            }
        }

        private void SetWorkersSticker()
        {
            if (OfficeRuntime.Instance.WorkersInOffice().Count == 0)
            {
                AddNone(_workersContainer);
            }
            else
            {
                for (int i = 0; i < OfficeRuntime.Instance.WorkersInOffice().Count; i++)
                {
                    AddObjectWithStyle(_workersContainer, "worker--pawn");
                }
            }
        }
        
        private void OnTaskChanged()
        {
            if (_task)
            {
                // Обновляем текстовую информацию.
                _taskName.text = _task.taskName;
                _taskDescription.text = _task.taskDescription;
                _workersRequired.text = $"MIN WORKERS REQUIRED: {_task.workerAmountRequired}.";
                _daysRequired.text = $"TASK WILL TAKE {_task.duration} DAYS.";
                
                // Обновляем ячейки навыков.
                UpdateSkillCells(_patienceCells, _task.patienceRequired);
                UpdateSkillCells(_socialCells, _task.socialRequired);
                UpdateSkillCells(_intellectualCells, _task.intellectualRequired);
                UpdateSkillCells(_physicalCells, _task.physicalRequired);
            }
            else
            {
                // Если task == null, очищаем информацию.
                ClearAllSkillCells();
                ClearAllSkillCells();
                _taskName.text = "";
                _taskDescription.text = "";
                _workersRequired.text = "";
                _daysRequired.text = "";
            }
        }
        
        /// <summary>
        /// Кеширование ячеек навыков из контейнеров.
        /// </summary>
        private void CacheSkillCells()
        {
            // Получаем ячейки из каждого контейнера.
            _patienceCells = _skillPatienceCellsContainer?.Children().ToList() ?? new List<VisualElement>();
            _socialCells = _skillSocialCellsContainer?.Children().ToList() ?? new List<VisualElement>();
            _intellectualCells = _skillIntellectualCellsContainer?.Children().ToList() ?? new List<VisualElement>();
            _physicalCells = _skillPhysicalCellsContainer?.Children().ToList() ?? new List<VisualElement>();
        }
        
        /// <summary>
        /// Заполнение ячеек по уровню навыка.
        /// </summary>
        /// <param name="cells">Список ячеек.</param>
        /// <param name="skillLevel">Уровень навыка.</param>
        private void UpdateSkillCells(List<VisualElement> cells, int skillLevel)
        {
            if (cells == null || cells.Count == 0) return;
            
            // Ограничиваем уровень навыка количеством ячеек.
            int filledCount = Mathf.Clamp(skillLevel, 0, cells.Count);
            
            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i] == null) continue;
                
                if (i < filledCount)
                {
                    // Заполненная ячейка.
                    cells[i].RemoveFromClassList("task--skill--cell--empty");
                    cells[i].AddToClassList("task--skill--cell--full");
                }
                else
                {
                    // Пустая ячейка.
                    cells[i].RemoveFromClassList("task--skill--cell--full");
                    cells[i].AddToClassList("task--skill--cell--empty");
                }
            }
        }
        
        /// <summary>
        /// Возвращение всех ячеек в пустое состояние.
        /// </summary>
        private void ClearAllSkillCells()
        {
            ClearSkillCells(_patienceCells);
            ClearSkillCells(_socialCells);
            ClearSkillCells(_intellectualCells);
            ClearSkillCells(_physicalCells);
        }
        
        private void ClearSkillCells(List<VisualElement> cells)
        {
            if (cells == null) return;
            
            foreach (var cell in cells)
            {
                if (cell != null)
                {
                    cell.RemoveFromClassList("task--skill--cell--full");
                    cell.AddToClassList("task--skill--cell--empty");
                }
            }
        }
        
        private void AddObjectWithStyle(VisualElement container, string style)
        {
            VisualElement element = new VisualElement { style = { flexGrow = 0 } };
            element.AddToClassList(style);
                
            container.Add(element);
        }
        
        private void AddNone(VisualElement container)
        {
            VisualElement none = new VisualElement { style = { flexGrow = 0 } };
            none.AddToClassList("none");
                
            container.Add(none);
        }
    }   
}