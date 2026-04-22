using Gameflow;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Menus
{
    public class MainMenuScreen : MonoBehaviour
    {
        // Кнопки.
        private Button _startGameButton;
        private Button _optionsButton;
        private Button _quitGameButton;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Кнопки.
            _startGameButton = root.Q<Button>("start_game_button");
            _optionsButton = root.Q<Button>("options_button");
            _quitGameButton = root.Q<Button>("quit_game_button");
            
            // Подписываемся на события:
            // Переходы на другие сцены.
            _startGameButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.HiringScene)));
            _optionsButton.RegisterCallback<ClickEvent>(_ => Debug.Log("Not Implemented."));
            _quitGameButton.RegisterCallback<ClickEvent>(_ => Debug.Log("Not Implemented."));
        }
    }
}