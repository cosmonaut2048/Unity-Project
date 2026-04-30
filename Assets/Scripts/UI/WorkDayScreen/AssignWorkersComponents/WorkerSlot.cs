using Runtime;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen.AssignWorkersComponents
{
    public class WorkerSlot
    {
        private VisualElement _slot;
        private bool _isTaken;
        private WorkerRuntime _worker;
        
        public WorkerSlot(VisualElement slot)
        {
            _slot = slot;
            _isTaken = false;
            _worker = null;
        }
        
        public VisualElement Slot => _slot;
        public bool IsTaken =>  _isTaken;
        public WorkerRuntime Worker => _worker;

        public void FillSlot(WorkerRuntime worker)
        {
            _worker = worker;
            _isTaken = true;
            SetSlotImage();
        }

        public void FreeSlot()
        {
            _worker = null;
            _isTaken = false;
            ClearSlotImage();
        }

        private void SetSlotImage()
        {
            _slot.style.backgroundImage = new StyleBackground(_worker.Worker.Appearance.IconSprite);
        }
        
        private void ClearSlotImage()
        {
            _slot.style.backgroundImage = null;
        }
    }
}