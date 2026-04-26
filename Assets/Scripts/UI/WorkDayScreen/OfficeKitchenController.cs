using System.Collections.Generic;
using System.Linq;
using Gameflow;
using Runtime;
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
        private List<VisualElement> _workers;

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
            _workers = workerSetter.CacheWorkers(_workersContainer);
            workerSetter.HideWorkers(_workers);
            workerSetter.SetAllWorkers(_workers, OfficeWorkerPlacement.Instance.WorkersInKitchen, OfficeWorkerPlacement.Instance.KitchenCapacity);
            
            // Подписываемся на события.
            _goBackButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.HallsScene)));
        }
    }
}