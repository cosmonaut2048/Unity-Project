using System.Collections.Generic;
using Core.DialogueLogic;
using Gameflow;
using Runtime;
using UI.WorkDayScreen.WorkersInOfficeComponents;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class ComputerScreenController : MonoBehaviour
    {
        // Контейнеры.
        private VisualElement _workerCallCardContainer;
        private VisualElement _scrollView;
        private VisualElement _messageContainer;
        
        // Кнопки.
        private VisualElement _backButton;
        
        // Работники.
        private List<WorkerInComputer> _workers;

        void Start()
        {
            WorkersInComputerSetter workersInComputerSetter = new WorkersInComputerSetter();
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Контейнеры.
            _workerCallCardContainer = root.Q<VisualElement>("Worker_Call_Card_Container");
            _scrollView = root.Q<VisualElement>("Worker_Scroll_View");
            _messageContainer = root.Q<VisualElement>("No_Workers_Message_Container");
            // Кнопки.
            _backButton = root.Q<VisualElement>("back_button");
            
            // Очищаем данные с экрана.
            workersInComputerSetter.ClearAllCards(_workerCallCardContainer);
            _scrollView.style.display = DisplayStyle.None;
            _messageContainer.style.display = DisplayStyle.None;
            
            // Настройка работников.
            _workers = new List<WorkerInComputer>();
            
            // Добавляем элементы.
            if (OfficeWorkerPlacement.Instance.WorkersInComputer.Count == 0)
            {
                _messageContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                _scrollView.style.display = DisplayStyle.Flex;
                workersInComputerSetter.CreateAllCards(_workers, OfficeWorkerPlacement.Instance.WorkersInComputer, _workerCallCardContainer);
            }
            
            // Подписываемся на события.
            _backButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.MainRoomScene)));
            foreach (var worker in _workers)
            {
                worker.SubscribeToClickEvents();
                worker.SubscribeToDialogueOnClickEvent(DialogueConditions.WorkDay, Scenes.ComputerScreenScene);
            }
        }
    }
}