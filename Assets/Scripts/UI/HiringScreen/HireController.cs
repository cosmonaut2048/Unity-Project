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
        
        private VisualElement _workerPortrait;
        private Label _workerName;
        
        private VisualElement _skillPatienceCellsContainer;
        private VisualElement _skillSocialCellsContainer;
        private VisualElement _skillIntellectualCellsContainer;
        private VisualElement _skillPhysicalCellsContainer;
        
        private List<VisualElement> _patienceCells;
        private List<VisualElement> _socialCells;
        private List<VisualElement> _intellectualCells;
        private List<VisualElement> _physicalCells;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue
            _workerPortrait = root.Q<VisualElement>("worker_portrait");
            _workerName = root.Q<Label>("worker_card_name");
            _skillPatienceCellsContainer = root.Q<VisualElement>("Skill_Patience_Cells_Container");
            _skillSocialCellsContainer = root.Q<VisualElement>("Skill_Social_Cells_Container");
            _skillIntellectualCellsContainer = root.Q<VisualElement>("Skill_Intellectual_Cells_Container");
            _skillPhysicalCellsContainer = root.Q<VisualElement>("Skill_Physical_Cells_Container");
            
            // Кэшируем ячейки навыков
            CacheSkillCells();
            
            // Инициализируем из сериализованного поля.
            _worker = worker;
            OnWorkerChanged();
        }
        
        // Для отслеживания в инспекторе.
        private void OnValidate()
        {
            if (Application.isPlaying && worker != _worker)
            {
                Worker = worker;
            }
        }
        
        private void CacheSkillCells()
        {
            // Получаем ячейки из каждого контейнера.
            _patienceCells = _skillPatienceCellsContainer?.Children().ToList() ?? new List<VisualElement>();
            _socialCells = _skillSocialCellsContainer?.Children().ToList() ?? new List<VisualElement>();
            _intellectualCells = _skillIntellectualCellsContainer?.Children().ToList() ?? new List<VisualElement>();
            _physicalCells = _skillPhysicalCellsContainer?.Children().ToList() ?? new List<VisualElement>();
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
            }
            else
            {
                // Если worker == null, очищаем ячейки.
                ClearAllSkillCells();
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
