using System.Collections.Generic;
using System.Linq;
using Content;
using Gameflow;
using Runtime;
using UI.WorkDayScreen.AssignWorkersComponents;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.TaskResultScreen
{
    public class TaskResultController : MonoBehaviour
    {
        // Контейнеры.
        private VisualElement _workerSlotsContainer;
        // Элементы контейнеров. 
        private List<WorkerSlot> _workerSlots = new List<WorkerSlot>();
        // Текстовые элементы.
        private Label _taskCompletionText;
        private Label _criticalSuccessText;
        private Label _criticalFailureText;
        // Кнопки.
        private Button _backButton;
        
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

        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Контейнеры.
            _workerSlotsContainer = root.Q<VisualElement>("Worker_Slots_Container");
            // Текстовые элементы.
            _taskCompletionText = root.Q<Label>("task_completion_text");
            _criticalSuccessText = root.Q<Label>("critical_success_text");
            _criticalFailureText = root.Q<Label>("critical_failure_text");
            // Кнопки.
            _backButton = root.Q<Button>("back_button");
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
            
            // Кэшируем.
            CacheSkillCells();
            CacheSlots();
            
            // Очищаем экран.
            ClearWorkerSlots();
            ClearAllSkillCells();
            
            // Расставляем элементы.
            SetWorkerIcons();
            SetTaskResultText();
            SetTaskPoster(OfficeRuntime.Instance.LastTaskResult.Task);
            
            // Подписываемся на события.
            _backButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.HallsScene)));
        }

        private void SwitchClasses(VisualElement element, string setClass, string removeClass)
        {
            if (element.ClassListContains(removeClass))
                _taskCompletionText.RemoveFromClassList(removeClass);
            
            element.AddToClassList(setClass);
        }

        private void SetTaskResultText()
        {
            if (OfficeRuntime.Instance.LastTaskResult.IsSuccess)
            {
                _taskCompletionText.text = "TASK COMPLETED SUCCESSFULLY";
                SwitchClasses(_taskCompletionText, "task--result--success", "task--result--failure");
            }
            else
            {
                _taskCompletionText.text = "TASK FAILED";
                SwitchClasses(_taskCompletionText, "task--result--failure", "task--result--success");
            }

            if (OfficeRuntime.Instance.LastTaskResult.IsCriticalFailure)
            {
                _criticalFailureText.text = "YES";
                SwitchClasses(_criticalFailureText, "critical--text--no", "critical--text--yes");
            }
            else
            {
                _criticalFailureText.text = "NO";
                SwitchClasses(_criticalFailureText, "critical--text--yes", "critical--text--no");
            }

            if (OfficeRuntime.Instance.LastTaskResult.IsCriticalSuccess)
            {
                _criticalSuccessText.text = "YES";
                SwitchClasses(_criticalSuccessText, "critical--text--yes", "critical--text--no");
            }
            else
            {
                _criticalSuccessText.text = "NO";
                SwitchClasses(_criticalSuccessText, "critical--text--no", "critical--text--yes");
            }
        }
        
        private void SetWorkerIcons()
        {
            List<WorkerRuntime> assignedWorkers = OfficeRuntime.Instance.LastTaskResult.Workers;
            
            ClearWorkerSlots();
            
            for (int i = 0; i < assignedWorkers.Count; i++)
            {
                _workerSlots[i].FillSlot(assignedWorkers[i]);
            }
                
            foreach (var slot in _workerSlots)
            {
                slot.Slot.pickingMode = PickingMode.Ignore;
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
        
        private void ClearWorkerSlots()
        {
            foreach (var slot in _workerSlots)
            {
                slot.FreeSlot();
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