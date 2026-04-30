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