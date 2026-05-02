using System.Collections.Generic;
using Gameflow;
using Runtime;
using UI.WorkDayScreen.WorkersInOfficeComponents;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class OfficeSecondRoomController : MonoBehaviour
    {
        private Button _goBackButton;
        // Контейнер работников.
        private VisualElement _workersContainer;
        // Работники.
        private List<WorkerInOffice> _workers = new List<WorkerInOffice>();

        void Start()
        {
            WorkerSetter workerSetter = new WorkerSetter();
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Кнопки.
            _goBackButton = root.Q<Button>("go_back_button");
            // Контейнер работников.
            _workersContainer = root.Q<VisualElement>("Workers_Container");
            
            // Настройка работников.
            _workers = workerSetter.CreateWorkersInOffice(OfficeWorkerPlacement.Instance.WorkersInSecondRoom, _workersContainer);
            workerSetter.HideAllWorkers(_workersContainer);
            workerSetter.SetAllWorkers(_workers, OfficeWorkerPlacement.Instance.SecondRoomCapacity);
            
            // Подписываемся на события.
            _goBackButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.HallsScene)));
            
            foreach (var worker in _workers)
            {
                worker.SubscribeToClickEvents();
            }
        }
    }
}