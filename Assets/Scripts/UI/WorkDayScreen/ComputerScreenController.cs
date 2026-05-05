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
            // Кнопки.
            _backButton = root.Q<VisualElement>("back_button");
            
            // Очищаем данные с экрана.
            workersInComputerSetter.ClearAllCards(_workerCallCardContainer);
            
            // Настройка работников.
            _workers = new List<WorkerInComputer>();
            
            // Добавляем элементы для работников.
            workersInComputerSetter.CreateAllCards(_workers, OfficeRuntime.Instance.WorkersInOffice(), _workerCallCardContainer);
            
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