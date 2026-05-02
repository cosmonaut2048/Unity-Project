using System.Collections.Generic;
using Core.DayLogic;
using Gameflow;
using Runtime;
using UI.WorkDayScreen.WorkersInOfficeComponents;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class OfficeHallController : MonoBehaviour
    {
        // Контейнеры интерактивных зон.
        private VisualElement _boardHoverContainer;
        private VisualElement _wallClockContainer;
        private VisualElement _mainRoomHoverContainer;
        private VisualElement _secondRoomHoverContainer;
        private VisualElement _kitchenHoverContainer;
        // Эффекты наведения мыши.
        private VisualElement _boardHoverGlow;
        private VisualElement _wallClockGlow;
        private VisualElement _mainRoomHoverGlow;
        private VisualElement _secondRoomHoverGlow;
        private VisualElement _kitchenHoverGlow;
        // Экран с запросом на завершение дня.
        private VisualElement _endDayPromptContainer;
        private Button _endDayButton;
        private Button _dontEndDayButton;
        // Контейнер работников.
        private VisualElement _workersContainer;
        // Работники.
        private List<WorkerInOffice> _workers = new List<WorkerInOffice>();

        void Start()
        {
            WorkerSetter workerSetter = new WorkerSetter();
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Контейнеры интерактивных зон.
            _boardHoverContainer = root.Q<VisualElement>("Board_Hover_Container");
            _wallClockContainer = root.Q<VisualElement>("Wall_Clock_Container");
            _mainRoomHoverContainer = root.Q<VisualElement>("Main_Room_Hover_Container");
            _secondRoomHoverContainer = root.Q<VisualElement>("Second_Room_Hover_Container");
            _kitchenHoverContainer = root.Q<VisualElement>("Kitchen_Hover_Container");
            // Эффекты наведения мыши.
            _boardHoverGlow = root.Q<VisualElement>("board_hover_glow");
            _wallClockGlow = root.Q<VisualElement>("wall_clock_glow");
            _mainRoomHoverGlow = root.Q<VisualElement>("main_room_hover_glow");
            _secondRoomHoverGlow = root.Q<VisualElement>("second_room_hover_glow");
            _kitchenHoverGlow = root.Q<VisualElement>("kitchen_hover_glow");
            // Экран с запросом на завершение дня.
            _endDayPromptContainer = root.Q<VisualElement>("End_Day_Prompt_Container");
            _endDayButton = root.Q<Button>("end_day_button");
            _dontEndDayButton = root.Q<Button>("dont_end_day_button");
            // Контейнер работников.
            _workersContainer = root.Q<VisualElement>("Workers_Container");
            
            // Скрываем неактивные элементы.
            _boardHoverGlow.style.display = DisplayStyle.None;
            _wallClockGlow.style.display = DisplayStyle.None;
            _mainRoomHoverGlow.style.display = DisplayStyle.None;
            _secondRoomHoverGlow.style.display = DisplayStyle.None;
            _kitchenHoverGlow.style.display = DisplayStyle.None;
            _endDayPromptContainer.style.display = DisplayStyle.None;
            
            // Настройка работников.
            _workers = workerSetter.CreateWorkersInOffice(OfficeWorkerPlacement.Instance.WorkersInHall, _workersContainer);
            workerSetter.HideAllWorkers(_workersContainer);
            workerSetter.SetAllWorkers(_workers, OfficeWorkerPlacement.Instance.HallCapacity);
            
            // Подписываемся на события.
            _boardHoverContainer.RegisterCallback<MouseEnterEvent>(_ => _boardHoverGlow.style.display = DisplayStyle.Flex);
            _boardHoverContainer.RegisterCallback<MouseLeaveEvent>(_ => _boardHoverGlow.style.display = DisplayStyle.None);
            
            _wallClockContainer.RegisterCallback<MouseEnterEvent>(_ => _wallClockGlow.style.display = DisplayStyle.Flex);
            _wallClockContainer.RegisterCallback<MouseLeaveEvent>(_ => _wallClockGlow.style.display = DisplayStyle.None);
            
            _mainRoomHoverContainer.RegisterCallback<MouseEnterEvent>(_ => _mainRoomHoverGlow.style.display = DisplayStyle.Flex);
            _mainRoomHoverContainer.RegisterCallback<MouseLeaveEvent>(_ => _mainRoomHoverGlow.style.display = DisplayStyle.None);
            
            _secondRoomHoverContainer.RegisterCallback<MouseEnterEvent>(_ => _secondRoomHoverGlow.style.display = DisplayStyle.Flex);
            _secondRoomHoverContainer.RegisterCallback<MouseLeaveEvent>(_ => _secondRoomHoverGlow.style.display = DisplayStyle.None);
            
            _kitchenHoverContainer.RegisterCallback<MouseEnterEvent>(_ => _kitchenHoverGlow.style.display = DisplayStyle.Flex);
            _kitchenHoverContainer.RegisterCallback<MouseLeaveEvent>(_ => _kitchenHoverGlow.style.display = DisplayStyle.None);
            
            _boardHoverContainer.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.BulletinBoardScene)));
            _wallClockContainer.RegisterCallback<ClickEvent>(OnClockClick);
            _kitchenHoverContainer.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.KitchenScene)));
            _mainRoomHoverContainer.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.MainRoomScene)));
            _secondRoomHoverContainer.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.SecondRoomScene)));
            _endDayButton.RegisterCallback<ClickEvent>(OnEndDayButtonClick);
            _dontEndDayButton.RegisterCallback<ClickEvent>(_ => _endDayPromptContainer.style.display = DisplayStyle.None);

            foreach (var worker in _workers)
            {
                worker.SubscribeToClickEvents();
            }
        }

        private void OnClockClick(ClickEvent evt)
        {
            _endDayPromptContainer.style.display = DisplayStyle.Flex;
        }

        private void OnEndDayButtonClick(ClickEvent evt)
        {
            DayCycleManager.Instance.OnDayEnd();
            SceneController.Instance.LoadScene(nameof(Scenes.StatsScene));
        }
    }
}