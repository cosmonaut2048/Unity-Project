using System.Collections.Generic;
using System.Linq;
using Content;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HiringScreen
{
    public class HireController : MonoBehaviour
    {
        [SerializeField] private WorkerDef worker;
        private WorkerDef _worker;
        
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

        void Start()
        {
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
            
            // Кэшируем ячейки навыков.
            CacheSkillCells();
            
            // Инициализируем из сериализованного поля.
            _worker = worker;
            OnWorkerChanged();
            
            // Скрываем элементы.
            _trait1InfoHover.style.display = DisplayStyle.None;
            _trait2InfoHover.style.display = DisplayStyle.None;
            
            _trait1InfoCell.RegisterCallback<MouseEnterEvent>(evt => OnInfoCellEnter(_trait1InfoHover));
            _trait1InfoHover.RegisterCallback<MouseOutEvent>(evt => OnInfoLeave(_trait1InfoHover));
    
            _trait2InfoCell.RegisterCallback<MouseEnterEvent>(evt => OnInfoCellEnter(_trait2InfoHover));
            _trait2InfoHover.RegisterCallback<MouseOutEvent>(evt => OnInfoLeave(_trait2InfoHover));
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
            if (Application.isPlaying && worker != _worker)
            {
                Worker = worker;
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
                // Если worker == null, очищаем ячейки.
                ClearAllSkillCells();
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
