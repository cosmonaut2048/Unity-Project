using Gameflow;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.WorkDayScreen
{
    public class OfficeMainRoomController : MonoBehaviour
    {
        private Button _goBackButton;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Кнопки.
            _goBackButton = root.Q<Button>("go_back_button");
            
            // Подписываемся на события.
            _goBackButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.HallsScene)));
        }
    }
}