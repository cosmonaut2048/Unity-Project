using Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace UI.MenuScreen
{
    public class DevMenuController : MonoBehaviour
    {
        [SerializeField] private int devMenuSortingOrder = 100; 
        [SerializeField] private InputActionReference esc;
        [SerializeField] private UIDocument uiDocument;
        private int _originalSortingOrder;
        
        // Контейнеры.
        private VisualElement _devMenuContainer;
        // Кнопки.
        private Button _resumeGameButton;
        private Button _optionsButton;
        private Button _saveButton;
        private Button _loadButton;
        private Button _saveQuitToMainMenuButton;
        private Button _saveQuitToDesktopButton;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Контейнеры.
            _devMenuContainer = root.Q<VisualElement>("Dev_Menu_Container");
            // Кнопки.
            _resumeGameButton = root.Q<Button>("resume_game_button");
            _optionsButton = root.Q<Button>("options_button");
            _saveButton = root.Q<Button>("save_button");
            _loadButton = root.Q<Button>("load_button");
            _saveQuitToMainMenuButton = root.Q<Button>("save_quit_to_main_menu_button");
            _saveQuitToDesktopButton = root.Q<Button>("save_quit_to_desktop_button");
            
            // Подписываемся на события:
            _saveButton.RegisterCallback<ClickEvent>(_ => SaveService.Instance.SaveGame());
            _loadButton.RegisterCallback<ClickEvent>(_ => LoadService.Instance.LoadGame());
            
            // Скрываем элементы.
            _devMenuContainer.style.display = DisplayStyle.None;
        }

        void Update()
        {
            if (esc.action.triggered)
            {
                Debug.Log("Escape Pressed");
                if (_devMenuContainer.style.display == DisplayStyle.None)
                {
                    _devMenuContainer.style.display = DisplayStyle.Flex;
                    uiDocument.sortingOrder = devMenuSortingOrder;
                    return;
                }
                
                _devMenuContainer.style.display = DisplayStyle.None;
                uiDocument.sortingOrder = _originalSortingOrder;
            }
        }
    }
}