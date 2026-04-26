using Gameflow;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class OfficeMainRoomController : MonoBehaviour
    {
        // Контейнеры интерактивных зон.
        private VisualElement _computerHoverContainer;
        // Эффекты наведения мыши.
        private VisualElement _computerHoverGlow;
        // Кнопки.
        private Button _goBackButton;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Контейнеры интерактивных зон.
            _computerHoverContainer = root.Q<VisualElement>("Computer_Hover_Container");
            // Эффекты наведения мыши.
            _computerHoverGlow = root.Q<VisualElement>("computer_hover_glow");
            // Кнопки.
            _goBackButton = root.Q<Button>("go_back_button");
            
            // Скрываем неактивные элементы.
            _computerHoverGlow.style.display = DisplayStyle.None;
            
            // Подписываемся на события.
            _computerHoverContainer.RegisterCallback<MouseEnterEvent>(_ => _computerHoverGlow.style.display = DisplayStyle.Flex);
            _computerHoverContainer.RegisterCallback<MouseLeaveEvent>(_ => _computerHoverGlow.style.display = DisplayStyle.None);
            _goBackButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.HallsScene)));
            
            _computerHoverContainer.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.ComputerScreenScene)));
        }
    }
}