using System.Collections.Generic;
using System.Linq;
using Content;
using Gameflow;
using Runtime;
using UI.WorkDayScreen.AssignWorkersComponents;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class AssignWorkersController : MonoBehaviour
    {
        // Контейнеры.
        private VisualElement _workerSlotsContainer;
        private VisualElement _workersContainer;
        private VisualElement _scrollContainer;
        // Кнопки.
        private Button _backButton;
        private Button _startTaskButton;
        // Элементы контейнеров.
        private List<WorkerIcon> _workerIcons = new List<WorkerIcon>();
        private List<WorkerSlot> _workerSlots = new List<WorkerSlot>();
        
        // Элементы задания:
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
        
        // Экран во время задания.
        private Label _onTaskText;
        
        // Экран ошибки.
        private VisualElement _assignmentWarning;
        private Button _tryAgainButton;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Контейнеры.
            _workerSlotsContainer = root.Q<VisualElement>("Worker_Slots_Container");
            _workersContainer = root.Q<VisualElement>("Workers_Container");
            _scrollContainer = root.Q<VisualElement>("Scroll_Container");
            // Кнопки.
            _backButton = root.Q<Button>("back_button");
            _startTaskButton = root.Q<Button>("start_task_button");
            // Элементы задания:
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
            // Экран во время задания.
            _onTaskText = root.Q<Label>("on_task_text");
            // Экран ошибки.
            _assignmentWarning = root.Q<VisualElement>("Assignment_Warning");
            _tryAgainButton = root.Q<Button>("try_again_button");
            
            // Кэшируем ячейки навыков.
            CacheSkillCells();
            
            // Очищаем экран.
            ClearAllSkillCells();
            ClearWorkerIcons();
            
            // Расставляем элементы.
            SetWorkerIcons();
            SetTaskPoster(OfficeRuntime.Instance.AvailableTask);
            
            // Получаем элементы.
            CacheSlots();
            
            // Подписываемся на события.
            _startTaskButton.RegisterCallback<ClickEvent>(OnStartTaskButton);
            _backButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.BulletinBoardScene)));
            _tryAgainButton.RegisterCallback<ClickEvent>(_ => HideWarning());
            
            foreach (var icon in _workerIcons)
            {
                icon.Icon.RegisterCallback<ClickEvent>(_ => OnIconClick(icon));
            }

            foreach (var slot in _workerSlots)
            {
                slot.Slot.RegisterCallback<ClickEvent>(_ => OnSlotClick(slot));
            }

            if (OfficeRuntime.Instance.CurrentTask)
                 OnTaskUI();
        }

        private void HideWarning()
        {
            _assignmentWarning.style.display = DisplayStyle.None;
        }

        private void OnStartTaskButton(ClickEvent clickEvent)
        {
            if (OfficeRuntime.Instance.CurrentTask)
            {
                return;
            }

            if (GetAssignedWorkers().Count < OfficeRuntime.Instance.AvailableTask.WorkerAmountRequired)
            {
                ShowAssignmentWarning();
                return;
            }

            OfficeRuntime.Instance.SetActiveTask(OfficeRuntime.Instance.AvailableTask, GetAssignedWorkers());
            OnTaskUI();
        }

        private void OnTaskUI()
        {
            List<WorkerRuntime> assignedWorkers = 
                OfficeRuntime.Instance.HiredWorkers.Where(worker => worker.BusyReason == BusyReason.OnTask).ToList();
            
            ClearWorkerSlots();
            
            for (int i = 0; i < assignedWorkers.Count; i++)
            {
                _workerSlots[i].FillSlot(assignedWorkers[i]);
            }
            
            _startTaskButton.style.display = DisplayStyle.None;
            _scrollContainer.style.display = DisplayStyle.None;
            foreach (var slot in _workerSlots)
            {
                slot.Slot.pickingMode = PickingMode.Ignore;
            }

            _workerSlotsContainer.AddToClassList("worker--slots--container--on--task");
            
            _onTaskText.style.display = DisplayStyle.Flex;
            
            SetTaskPoster(OfficeRuntime.Instance.CurrentTask.Task);
        }

        private List<WorkerRuntime> GetAssignedWorkers()
        {
            List<WorkerRuntime> assignedWorkers = new List<WorkerRuntime>();
            
            foreach (var workerSlot in _workerSlots)
            {
                if (workerSlot.IsTaken)
                    assignedWorkers.Add(workerSlot.Worker);
            }
            
            return assignedWorkers;
        }

        private void ShowAssignmentWarning()
        {
            _assignmentWarning.style.display = DisplayStyle.Flex;
        }

        private void OnIconClick(WorkerIcon icon)
        {
            Debug.Log("Icon clicked");
            var slot = GetFirstEmptySlot();
            
            if (slot == null || icon.IsEmpty) return;
            
            slot.FillSlot(icon.Worker);
            icon.FreeIcon();
        }

        private void OnSlotClick(WorkerSlot slot)
        {
            Debug.Log("Slot clicked");
            if (!slot.Worker) return;
            
            foreach (var icon in _workerIcons)
            {
                if (slot.Worker == icon.Worker)
                {
                    slot.FreeSlot();
                    icon.FillIcon();
                }
            }
        }

        private WorkerSlot GetFirstEmptySlot()
        {
            foreach (var slot in _workerSlots)
            {
                if (!slot.IsTaken)
                {
                    Debug.Log("Slot found");
                    return slot;
                }
            }
            
            return null;
        }

        private void SetWorkerIcons()
        {
            foreach (var worker in OfficeRuntime.Instance.WorkersInOffice())
            {
                var portraitImage = new StyleBackground(worker.Worker.Appearance.IconSprite);
                VisualElement icon = new VisualElement { style = { flexGrow = 0, backgroundImage = portraitImage } };
                icon.AddToClassList("worker--icon");
                
                // Создаём контейнер иконки.
                VisualElement iconContainer = new VisualElement { style = { flexGrow = 0 } };
                iconContainer.AddToClassList("worker--icon--container");
                
                // Добавляем иконку в контейнер иконки.
                iconContainer.Add(icon);
                // Добавляем контейнер с иконкой в главный контейнер.
                _workersContainer.Add(iconContainer);
                
                // Добавляем в список WorkerIcon.
                WorkerIcon workerIcon = new WorkerIcon(icon, worker);
                _workerIcons.Add(workerIcon);
            }
        }

        private void ClearWorkerIcons()
        {
            _workerIcons?.Clear();
            _workersContainer?.Clear();
        }

        private void ClearWorkerSlots()
        {
            foreach (var slot in _workerSlots)
            {
                slot.FreeSlot();
            }
        }

        private void CacheSlots()
        {
            _workerSlots.Clear();
            
            if (_workerSlotsContainer?.Children() == null) return;
            
            foreach (var slotContainer in _workerSlotsContainer.Children().ToList())
            {
                foreach (var element in slotContainer.Children().ToList())
                {
                    if (element.ClassListContains("slot"))
                        _workerSlots.Add(new WorkerSlot(element));
                }
            }
        }
        
        private void SetTaskPoster(TaskDef task)
        {
            if (task)
            {
                // Обновляем текстовую информацию.
                _taskName.text = task.TaskName;
                _taskDescription.text = task.TaskDescription;
                _workersRequired.text = $"MIN WORKERS REQUIRED: {task.WorkerAmountRequired}.";
                _daysRequired.text = $"TASK WILL TAKE {task.Duration} DAYS.";
                
                // Обновляем ячейки навыков.
                UpdateSkillCells(_patienceCells, task.PatienceRequired);
                UpdateSkillCells(_socialCells, task.SocialRequired);
                UpdateSkillCells(_intellectualCells, task.IntellectualRequired);
                UpdateSkillCells(_physicalCells, task.PhysicalRequired);
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
    }
}