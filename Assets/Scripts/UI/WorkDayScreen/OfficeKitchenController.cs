using System.Collections.Generic;
using System.Linq;
using Core.DialogueLogic;
using Gameflow;
using Runtime;
using UI.WorkDayScreen.WorkersInOfficeComponents;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class OfficeKitchenController : MonoBehaviour
    {
        private Button _goBackButton;
        // Контейнер работников.
        private VisualElement _workersContainer;
        // Работники.
        private List<WorkerInOffice> _workers = new List<WorkerInOffice>();

        void Start()
        {
            WorkersInOfficeSetter workersInOfficeSetter = new WorkersInOfficeSetter();
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Кнопки.
            _goBackButton = root.Q<Button>("go_back_button");
            // Контейнер работников.
            _workersContainer = root.Q<VisualElement>("Workers_Container");
            
            // Настройка работников.
            _workers = workersInOfficeSetter.CreateWorkersInOffice(OfficeWorkerPlacement.Instance.WorkersInKitchen, _workersContainer);
            workersInOfficeSetter.HideAllWorkers(_workersContainer);
            workersInOfficeSetter.SetAllWorkers(_workers, OfficeWorkerPlacement.Instance.KitchenCapacity);
            
            // Подписываемся на события.
            _goBackButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.HallsScene)));
            
            foreach (var worker in _workers)
            {
                worker.SubscribeToClickEvents();
                worker.SubscribeToDialogueOnClickEvent(DialogueConditions.WorkDay, Scenes.KitchenScene);
            }
        }
    }
}