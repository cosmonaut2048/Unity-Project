using System.Collections.Generic;
using System.Linq;
using Gameflow;
using Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class ComputerScreenController : MonoBehaviour
    {
        // Контейнеры.
        private VisualElement _workerCallCardContainer;
        
        // Кнопки.
        private VisualElement _backButton;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Контейнеры.
            _workerCallCardContainer = root.Q<VisualElement>("Worker_Call_Card_Container");
            // Кнопки.
            _backButton = root.Q<VisualElement>("back_button");
            
            // Очищаем данные с экрана.
            ClearScreen();
            
            // Добавляем элементы для работников.
            foreach (var worker in OfficeRuntime.Instance.WorkersInOffice())
            {
                CreateCard(worker);
            }
            
            // Подписываемся на события.
            _backButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.MainRoomScene)));
        }

        private void CreateCard(WorkerRuntime worker)
        {
            // Создаём контейнеры.
            VisualElement workerCallCard = new VisualElement();
            VisualElement workerCallInfo = new VisualElement();
            VisualElement giveButtonsContainer = new VisualElement();
            
            // Создаём элементы.
            VisualElement workerIcon = new VisualElement();
            Label workerName = new Label();
            Button callButton = new Button();
            Button giveCoffeeButton = new Button();
            Button giveBreakButton = new Button();
            
            // Применяем стили.
            workerCallCard.AddToClassList("worker--call--card");
            workerCallInfo.AddToClassList("worker--call--info");
            giveButtonsContainer.AddToClassList("give--buttons--container");
            workerIcon.AddToClassList("worker--icon");
            workerName.AddToClassList("worker--name");
            callButton.AddToClassList("call--button");
            giveCoffeeButton.AddToClassList("coffee--button");
            giveBreakButton.AddToClassList("break--button");
            
            // Добавляем информацию работника.
            workerIcon.style.backgroundImage = new StyleBackground(worker.Worker.Appearance.IconSprite);
            workerName.text = worker.Worker.Appearance.WorkerName;
            callButton.text = "Call";
            giveCoffeeButton.text = "Give Coffee";
            giveBreakButton.text = "Give Break";
            
            // Добавляем элементы в контейнеры.
            giveButtonsContainer.Add(giveCoffeeButton);
            giveButtonsContainer.Add(giveBreakButton);
            
            workerCallInfo.Add(workerName);
            workerCallInfo.Add(callButton);
            workerCallInfo.Add(giveButtonsContainer);
            
            workerCallCard.Add(workerIcon);
            workerCallCard.Add(workerCallInfo);
            
            _workerCallCardContainer.Add(workerCallCard);
        }

        private void ClearScreen()
        {
            List<VisualElement> elements = _workerCallCardContainer?.Children().ToList();
            
            if (elements == null) return;
            
            foreach (var element in elements)
            {
                if (element != null)
                {
                    _workerCallCardContainer.Remove(element);
                }
            }
        }
    }
}