using Runtime;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen.WorkersInOfficeComponents
{
    public class WorkerInOffice
    {
        private readonly VisualElement _workerContainer;
        private readonly VisualElement _workerSprite;
        private readonly VisualElement _giveCoffeeElement;
        private readonly VisualElement _giveVoucherElement;
        
        private readonly WorkerRuntime _worker;
        public WorkerInOffice(WorkerRuntime worker, VisualElement workerContainer, VisualElement workerSprite,  VisualElement giveCoffeeElement, VisualElement giveVoucherElement)
        {
            _worker = worker;
            _workerContainer = workerContainer;
            _workerSprite = workerSprite;
            _giveCoffeeElement  = giveCoffeeElement;
            _giveVoucherElement =  giveVoucherElement;
        }
        
        private void SetGivenCoffee()
        {
            _giveCoffeeElement.AddToClassList("give--coffee--inactive");
            _giveCoffeeElement.pickingMode = PickingMode.Ignore;
        }

        private void SetGivenVoucher()
        {
            _giveVoucherElement.AddToClassList("give--voucher--inactive");
            _giveVoucherElement.pickingMode = PickingMode.Ignore;
        }
        
        public void SetWorker()
        {
            _workerSprite.style.backgroundImage = new StyleBackground(_worker.Worker.Appearance.FullBodySprite);
            
            _workerContainer.pickingMode = PickingMode.Position;
            _workerContainer.style.display = DisplayStyle.Flex;
            
            if (OfficeRuntime.Instance.Coffee <= 0 || _worker.DrankCoffeeToday)
                SetGivenCoffee();
            if (OfficeRuntime.Instance.BreakVouchers <= 0 || _worker.TookBreakToday)
                SetGivenVoucher();
        }

        public void HideWorker()
        {
            _workerContainer.style.display = DisplayStyle.None;
            _workerContainer.pickingMode = PickingMode.Ignore;
        }

        public void SubscribeToClickEvents()
        {
            _giveCoffeeElement.RegisterCallback<ClickEvent>(OnGiveCoffee);
            _giveVoucherElement.RegisterCallback<ClickEvent>(OnGiveVoucher);
        }
        
        private void OnGiveCoffee(ClickEvent evt)
        {
            SetGivenCoffee();
            _worker.DrinkCoffee();
        }

        private void OnGiveVoucher(ClickEvent evt)
        {
            SetGivenVoucher();
            _worker.TakeBreak();
        }
    }
}