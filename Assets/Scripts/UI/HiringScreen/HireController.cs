using System.Collections.Generic;
using System.Linq;
using Content;
using Gameflow;
using Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HiringScreen
{
    public class HireController : MonoBehaviour
    {
        [SerializeField] private List<WorkerDef> candidates;
        private List<WorkerRuntime> _hiredWorkers;
        private WorkerDef _worker;
        private int _currentCandidateIndex;
        
        public WorkerDef Worker
        {
            get => _worker;
            set
            {
                if (_worker != value)
                {
                    _worker = value;
                    OnWorkerChanged();
                }
            }
        }
        
        public List<WorkerRuntime> HiredWorkers => _hiredWorkers;
        
        // Спрайт портрета и имя работника.
        private VisualElement _workerPortrait;
        private Label _workerName;
        
        // Контейнеры навыков.
        private VisualElement _skillPatienceCellsContainer;
        private VisualElement _skillSocialCellsContainer;
        private VisualElement _skillIntellectualCellsContainer;
        private VisualElement _skillPhysicalCellsContainer;
        
        // Название черты, информация о черте.
        private Label _trait1Name;
        private VisualElement _trait1InfoCell;
        private VisualElement _trait1InfoHover;
        private Label _trait1Description;
        
        private Label _trait2Name;
        private VisualElement _trait2InfoCell;
        private VisualElement _trait2InfoHover;
        private Label _trait2Description;
        
        // Ячейки навыков (получаем из контейнеров).
        private List<VisualElement> _patienceCells;
        private List<VisualElement> _socialCells;
        private List<VisualElement> _intellectualCells;
        private List<VisualElement> _physicalCells;
        
        // Кнопки.
        private Button _acceptButton;
        private Button _denyButton;

        void Start()
        {
            _hiredWorkers = new List<WorkerRuntime>();
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Спрайт портрета и имя работника.
            _workerPortrait = root.Q<VisualElement>("worker_portrait");
            _workerName = root.Q<Label>("worker_card_name");
            // Навыки.
            _skillPatienceCellsContainer = root.Q<VisualElement>("Skill_Patience_Cells_Container");
            _skillSocialCellsContainer = root.Q<VisualElement>("Skill_Social_Cells_Container");
            _skillIntellectualCellsContainer = root.Q<VisualElement>("Skill_Intellectual_Cells_Container");
            _skillPhysicalCellsContainer = root.Q<VisualElement>("Skill_Physical_Cells_Container");
            // Черты.
            _trait1Name = root.Q<Label>("trait_1_name");
            _trait1InfoCell = root.Q<VisualElement>("trait_1_info_cell");
            _trait1InfoHover =  root.Q<VisualElement>("trait_1_info_hover");
            _trait1Description = root.Q<Label>("trait_1_description");
            
            _trait2Name = root.Q<Label>("trait_2_name");
            _trait2InfoCell = root.Q<VisualElement>("trait_2_info_cell");
            _trait2InfoHover =  root.Q<VisualElement>("trait_2_info_hover");
            _trait2Description = root.Q<Label>("trait_2_description");
            // Кнопки.
            _acceptButton = root.Q<Button>("accept_button");
            _denyButton = root.Q<Button>("deny_button");
            
            // Кэшируем ячейки навыков.
            CacheSkillCells();
            
            // Показываем первого кандидата.
            if (candidates is { Count: > 0 })
            {
                _currentCandidateIndex = 0;
                _worker = candidates[_currentCandidateIndex];
                OnWorkerChanged();
            }
            
            // Скрываем элементы.
            _trait1InfoHover.style.display = DisplayStyle.None;
            _trait2InfoHover.style.display = DisplayStyle.None;
            
            // Подписываемся на события.
            _trait1InfoCell.RegisterCallback<MouseEnterEvent>(_ => OnInfoCellEnter(_trait1InfoHover));
            _trait1InfoHover.RegisterCallback<MouseOutEvent>(_ => OnInfoLeave(_trait1InfoHover));
    
            _trait2InfoCell.RegisterCallback<MouseEnterEvent>(_ => OnInfoCellEnter(_trait2InfoHover));
            _trait2InfoHover.RegisterCallback<MouseOutEvent>(_ => OnInfoLeave(_trait2InfoHover));
            
            _acceptButton.RegisterCallback<ClickEvent>(OnWorkerHired);
            _denyButton.RegisterCallback<ClickEvent>(OnWorkerDenied);
        }

        private void OnWorkerHired(ClickEvent evt)
        {
            if (!_worker) return;
    
            // Создаём WorkerRuntime на основе текущего кандидата.
            WorkerRuntime hiredWorker = ScriptableObject.CreateInstance<WorkerRuntime>();
            hiredWorker.InitializeWorkerRuntime(_worker);
    
            // Добавляем в список нанятых.
            _hiredWorkers.Add(hiredWorker);
    
            Debug.Log($"Worker hired: {_worker.name}");
    
            // Переходим к следующему кандидату.
            ShowNextCandidate();
        }

        private void OnWorkerDenied(ClickEvent evt)
        {
            if (!_worker) return;
    
            Debug.Log($"Worker denied: {_worker.name}");
    
            // Переходим к следующему кандидату
            ShowNextCandidate();
        }
        
        private void ShowNextCandidate()
        {
            _currentCandidateIndex++;
            
            if (_currentCandidateIndex < candidates.Count)
            {
                Worker = candidates[_currentCandidateIndex];
            }
            else
            {
                Debug.Log("Out of candidates.");
                OnAllCandidatesReviewed();
            }
        }
        
        private void OnAllCandidatesReviewed()
        {
            _worker = null;
            OnWorkerChanged();
            
            _acceptButton.SetEnabled(false);
            _denyButton.SetEnabled(false);
            
            _workerName.text = "No more candidates.";
            _workerPortrait.style.backgroundImage = null;
            
            App app = FindFirstObjectByType<App>();
        
            if (app)
            {
                OfficeRuntime.Instance.AddWorkers(_hiredWorkers);

                // Сообщение в консоль.
                string debugMessage = "Hired workers:";
                if (_hiredWorkers.Count > 0)
                {
                    foreach (var worker in _hiredWorkers)
                        debugMessage += " " + worker.Appearance.WorkerName;
                }
                else
                {
                    debugMessage += " None";
                }
                Debug.Log(debugMessage);
            }
            else
            {
                Debug.LogError("App not found!");
            }
        }

        private void OnInfoCellEnter(VisualElement targetHover)
        {
            targetHover.style.display = DisplayStyle.Flex;
            Debug.Log("OnInfoCellEnter");
        }

        private void OnInfoLeave(VisualElement targetHover)
        {
            targetHover.style.display = DisplayStyle.None;
            Debug.Log("OnInfoLeave");
        }
        
        /// <summary>
        /// Для отслеживания изменений в инспекторе.
        /// </summary>
        private void OnValidate()
        {
            if (Application.isPlaying && candidates is { Count: > 0 })
            {
                // Проверяем, что текущий индекс валидный.
                if (_currentCandidateIndex >= 0 && _currentCandidateIndex < candidates.Count)
                {
                    if (candidates[_currentCandidateIndex] != _worker)
                    {
                        Worker = candidates[_currentCandidateIndex];
                    }
                }
            }
        }
        
        private void OnWorkerChanged()
        {
            if (_workerPortrait == null || _workerName == null) return;
            
            if (_worker && _worker.Appearance)
            {
                _workerPortrait.style.backgroundImage = new StyleBackground(_worker.Appearance.PortraitSprite);
                _workerName.text = _worker.name;
                
                // Обновляем ячейки навыков.
                UpdateSkillCells(_patienceCells, _worker.BasePatience);
                UpdateSkillCells(_socialCells, _worker.BaseSocial);
                UpdateSkillCells(_intellectualCells, _worker.BaseIntellectual);
                UpdateSkillCells(_physicalCells, _worker.BasePhysical);
                
                // Обновляем информацию о чертах.
                _trait1Name.text = _worker.PersonalityTraits[0]?.traitName;
                _trait1Description.text = _worker.PersonalityTraits[0]?.traitDescription;
                
                _trait2Name.text = _worker.PersonalityTraits[1]?.traitName;
                _trait2Description.text = _worker.PersonalityTraits[1]?.traitDescription;
            }
            else
            {
                // Если worker == null, очищаем информацию.
                ClearAllSkillCells();
                ClearAllSkillCells();
                _trait1Name.text = "";
                _trait1Description.text = "";
                _trait2Name.text = "";
                _trait2Description.text = "";
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
                    cells[i].RemoveFromClassList("skill--cell--empty");
                    cells[i].AddToClassList("skill--cell--full");
                }
                else
                {
                    // Пустая ячейка.
                    cells[i].RemoveFromClassList("skill--cell--full");
                    cells[i].AddToClassList("skill--cell--empty");
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
                    cell.RemoveFromClassList("skill--cell--full");
                    cell.AddToClassList("skill--cell--empty");
                }
            }
        }
        
    }
}