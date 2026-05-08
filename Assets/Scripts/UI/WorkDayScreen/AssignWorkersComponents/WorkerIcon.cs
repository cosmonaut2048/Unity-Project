using System.Numerics;
using Runtime;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen.AssignWorkersComponents
{
    public class WorkerIcon
    {
        private VisualElement _icon;
        private WorkerRuntime _worker;
        private bool _isEmpty;

        public WorkerIcon(VisualElement icon, WorkerRuntime worker)
        {
            _icon = icon;
            _worker = worker;
            _isEmpty = false;
        }

        public VisualElement CreateInfo()
        {
            int cellAmount = 5;
            
            VisualElement container = new VisualElement { style = { flexGrow = 0 } };
            container.AddToClassList("worker--skills--container");

            /* ------------------------------ Имя работника ------------------------------ */
            Label workerName = new Label
            {
                text = _worker.Worker.Appearance.WorkerName,
                style = { flexGrow = 0 }
            };
            workerName.AddToClassList("worker--name");
            
            VisualElement nameContainer = new VisualElement { style = { flexGrow = 0 } };
            nameContainer.AddToClassList("worker--info--container");
            nameContainer.Add(workerName);
            container.Add(nameContainer);
            
            /* ------------------------------ Skills ------------------------------ */
            container.Add(CreateCells(cellAmount, "PATIENCE", _worker.Worker.BasePatience));
            container.Add(CreateCells(cellAmount, "SOCIAL", _worker.Worker.BaseSocial));
            container.Add(CreateCells(cellAmount, "INTELLECTUAL", _worker.Worker.BaseIntellectual));
            container.Add(CreateCells(cellAmount, "PHYSICAL", _worker.Worker.BasePhysical));
            
            return container;
        }
        
        private Vector2 GetGlobalPosition(VisualElement parentContainer)
        {
            var worldBound = parentContainer.worldBound;
            return new Vector2(worldBound.x, worldBound.y);
        }

        private VisualElement CreateCells(int cellAmount, string nameOfSkill, int skillLevel)
        {
            VisualElement infoContainer = new VisualElement { style = { flexGrow = 0 } };
            infoContainer.AddToClassList("worker--info--container");
            
            Label skillName = new Label { text = nameOfSkill, style = { flexGrow = 0 } };
            skillName.AddToClassList("worker--info--text");
            infoContainer.Add(skillName);
            
            VisualElement cellsContainer = new VisualElement { style = { flexGrow = 0 } };
            cellsContainer.AddToClassList("worker--cell--container");
            
            for (int i = 0; i < cellAmount; i++)
            {
                VisualElement cell = new VisualElement { style = { flexGrow = 0 } };

                cell.AddToClassList(i + 1 <= skillLevel
                    ? "worker--skill--cell--full"
                    : "worker--skill--cell--empty");

                cellsContainer.Add(cell);
            }
            
            infoContainer.Add(cellsContainer);
            
            return infoContainer;
        }
        
        public VisualElement Icon => _icon;
        public WorkerRuntime Worker => _worker;
        public bool IsEmpty => _isEmpty;

        public void FillIcon()
        {
            _icon.style.backgroundImage = new StyleBackground(_worker.Worker.Appearance.IconSprite);
            _isEmpty = false;
        }

        public void FreeIcon()
        {
            _icon.style.backgroundImage = null;
            _isEmpty = true;
        }
    }
}