using Core.DialogueLogic;
using Gameflow;
using Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen.WorkersInOfficeComponents
{
    public class WorkerInComputer
    {
        // Контейнеры.
        private VisualElement _workerCallCard;
        private VisualElement _workerCallInfo;
        private VisualElement _giveButtonsContainer;
            
        // Элементы.
        private VisualElement _workerIcon;
        private Label _workerName;
        private Button _callButton;
        private Button _giveCoffeeButton;
        private Button _giveBreakButton;
        
        private readonly WorkerRuntime _worker;

        public WorkerInComputer(WorkerRuntime worker)
        {
            _worker = worker;
        }
        
        public void CreateCard(VisualElement workerCallCardContainer)
        {
            // Создаём контейнеры.
            _workerCallCard = new VisualElement();
            _workerCallInfo = new VisualElement();
            _giveButtonsContainer = new VisualElement();
            
            // Создаём элементы.
            _workerIcon = new VisualElement();
            _workerName = new Label();
            _callButton = new Button();
            _giveCoffeeButton = new Button();
            _giveBreakButton = new Button();
            
            // Применяем стили.
            _workerCallCard.AddToClassList("worker--call--card");
            _workerCallInfo.AddToClassList("worker--call--info");
            _giveButtonsContainer.AddToClassList("give--buttons--container");
            _workerIcon.AddToClassList("worker--icon");
            _workerName.AddToClassList("worker--name");
            _callButton.AddToClassList("call--button");
            _giveCoffeeButton.AddToClassList("coffee--button");
            _giveBreakButton.AddToClassList("break--button");
            
            // Добавляем информацию работника.
            _workerIcon.style.backgroundImage = new StyleBackground(_worker.Worker.Appearance.IconSprite);
            _workerName.text = _worker.Worker.Appearance.WorkerName;
            _callButton.text = "Call";
            
            // Добавляем элементы в контейнеры.
            _giveButtonsContainer.Add(_giveCoffeeButton);
            _giveButtonsContainer.Add(_giveBreakButton);
            
            _workerCallInfo.Add(_workerName);
            _workerCallInfo.Add(_callButton);
            _workerCallInfo.Add(_giveButtonsContainer);
            
            _workerCallCard.Add(_workerIcon);
            _workerCallCard.Add(_workerCallInfo);
            
            if (OfficeRuntime.Instance.Coffee <= 0 || _worker.DrankCoffeeToday)
                SetGivenCoffee();
            if (OfficeRuntime.Instance.BreakVouchers <= 0 || _worker.TookBreakToday)
                SetGivenVoucher();
            
            workerCallCardContainer.Add(_workerCallCard);
        }
        
        public void SubscribeToDialogueOnClickEvent(DialogueConditions condition, Scenes scene, Texture2D background)
        {
            _callButton.RegisterCallback<ClickEvent>(_ => StartDialogue(condition, scene, background));
        }
        
        public void SubscribeToClickEvents()
        {
            _giveCoffeeButton.RegisterCallback<ClickEvent>(OnGiveCoffee);
            _giveBreakButton.RegisterCallback<ClickEvent>(OnGiveVoucher);
        }
        
        private void StartDialogue(DialogueConditions condition, Scenes scene, Texture2D background)
        {
            DialogueContext.Instance.SetDialogueContext(_worker, condition, scene);
            DialogueContext.Instance.SetDialogueBackground(background);
            SceneController.Instance.LoadScene(nameof(Scenes.DialogueScene));
        }
        
        private void SetGivenCoffee()
        {
            _giveCoffeeButton.AddToClassList("coffee--button--inactive");
            _giveCoffeeButton.pickingMode = PickingMode.Ignore;
        }

        private void SetGivenVoucher()
        {
            _giveBreakButton.AddToClassList("break--button--inactive");
            _giveBreakButton.pickingMode = PickingMode.Ignore;
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