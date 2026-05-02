using System.Collections.Generic;
using System.Linq;
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
            
            // Кэшируем.
            CacheSkillCells();
            CacheSlots();
            
            // Очищаем экран.
            ClearWorkerSlots();
            ClearAllSkillCells();
            
            // Расставляем элементы.
            SetWorkerIcons();
            SetTaskResultText();
            
            // Подписываемся на события.
            _backButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.HallsScene)));
        }

        private void SetTaskResultText()
        {
            _taskCompletionText.ClearClassList();
            
            if (OfficeRuntime.Instance.LastTaskResult.IsSuccess)
            {
                _taskCompletionText.text = "TASK COMPLETED SUCCESSFULLY";
                _taskCompletionText.AddToClassList("task--result--success");
            }
            else
            {
                _taskCompletionText.text = "TASK FAILED";
                _taskCompletionText.AddToClassList("task--result--failure");
            }
            
            _criticalFailureText.ClearClassList();

            if (OfficeRuntime.Instance.LastTaskResult.IsCriticalFailure)
            {
                _criticalFailureText.text = "YES";
                _criticalFailureText.AddToClassList("critical--text--no");
            }
            else
            {
                _criticalFailureText.text = "NO";
                _criticalFailureText.AddToClassList("critical--text--yes");
            }
            
            _criticalSuccessText.ClearClassList();

            if (OfficeRuntime.Instance.LastTaskResult.IsCriticalSuccess)
            {
                _criticalSuccessText.text = "YES";
                _criticalSuccessText.AddToClassList("critical--text--yes");
            }
            else
            {
                _criticalSuccessText.text = "NO";
                _criticalSuccessText.AddToClassList("critical--text--no");
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