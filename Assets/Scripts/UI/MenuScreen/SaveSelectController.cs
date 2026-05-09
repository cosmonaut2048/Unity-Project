using System.Collections.Generic;
using System.Linq;
using Gameflow;
using Runtime;
using Services;
using Services.SaveSlotComponents;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.MenuScreen
{
    public class SaveSelectController : MonoBehaviour
    {
        private VisualElement _saveSelectContainer;
        private VisualElement _confirmDeleteContainer;

        private Label _deleteText;
        private Button _yesDeleteButton;
        private Button _noDeleteButton;
        
        private Button _goBackButton;
        
        private List<Button> _saveSelectButtons;
        private List<Button> _saveDeleteButtons;
        
        private string _currentSlotToDelete;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            _saveSelectContainer = root.Q<VisualElement>("Save_Select_Container");
            _confirmDeleteContainer = root.Q<VisualElement>("Confirm_Delete_Container");
            
            _deleteText = root.Q<Label>("delete_text");
            _yesDeleteButton = root.Q<Button>("yes_delete_button");
            _noDeleteButton =  root.Q<Button>("no_delete_button");
            
            _goBackButton = root.Q<Button>("go_back_button");
            
            // Кешируем кнопки.
            CacheButtons();
            
            // Очищаем экран.
            ClearButtons();
            _confirmDeleteContainer.style.display = DisplayStyle.None;
            
            // Расставляем элементы.
            SetButtons();

            // Подписываемся на события.
            _goBackButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(nameof(Scenes.MainMenuScene)));
            
            foreach (var button in _saveSelectButtons)
            {
                button.RegisterCallback<ClickEvent>(_ => OnSaveSelectClick(button));
            }
            
            foreach (var button in _saveDeleteButtons)
            {
                button.RegisterCallback<ClickEvent>(_ => OnDeleteButtonClick(button));
            }
            
            _yesDeleteButton.RegisterCallback<ClickEvent>(_ => OnYesDeleteClick());
            _noDeleteButton.RegisterCallback<ClickEvent>(_ => OnNoDeleteClick());
        }
        
        private void OnSaveSelectClick(Button button)
        {
            string buttonText = button.text;

            if (buttonText == "New Game")
            {
                SaveSlotData newSlot = SaveSlotService.Instance.CreateNewSlot($"Game{_saveSelectButtons.IndexOf(button) + 1}");
        
                if (newSlot == null)
                {
                    Debug.LogError("Failed to create new slot!");
                    return;
                }
        
                SaveSlotService.Instance.SelectSlot(newSlot.SaveSlotName);
        
                OfficeWorkerPlacement.Instance.ClearAllRooms();
                OfficeRuntime.Instance.ClearOffice();
        
                SceneController.Instance.LoadScene(nameof(Scenes.HiringScene));
                return;
            }
    
            LoadService.Instance.LoadGameFromSlot(buttonText);
            SceneController.Instance.LoadScene(nameof(Scenes.HallsScene));
        }
        
        private void OnDeleteButtonClick(Button button)
        {
            _currentSlotToDelete = button.userData?.ToString();
            _deleteText.text = $"Delete {_currentSlotToDelete}?";
            _confirmDeleteContainer.style.display = DisplayStyle.Flex;
        }

        private void OnYesDeleteClick()
        {
            if (!string.IsNullOrEmpty(_currentSlotToDelete))
            {
                SaveSlotService.Instance.DeleteSlot(_currentSlotToDelete);
                ClearButtons();
                SetButtons();
            }
    
            _confirmDeleteContainer.style.display = DisplayStyle.None;
            _currentSlotToDelete = null;
        }

        private void OnNoDeleteClick()
        {
            _confirmDeleteContainer.style.display = DisplayStyle.None;
            _currentSlotToDelete = null;
        }
        
        private void SetButtons()
        {
            var saveSlots = SaveSlotService.Instance.SlotsData.SaveSlots;
    
            if (saveSlots.Count > _saveSelectButtons.Count)
                Debug.Log("More save slots than save select buttons.");
            
            for (int i = 0; i < _saveSelectButtons.Count; i++)
            {
                _saveSelectButtons[i].text = "New Game";
                _saveSelectButtons[i].style.width = new StyleLength(Length.Percent(100));
        
                if (i < _saveDeleteButtons.Count)
                {
                    _saveDeleteButtons[i].text = null;
                    _saveDeleteButtons[i].style.display = DisplayStyle.None;
                }
            }
            
            for (int i = 0; i < saveSlots.Count; i++)
            {
                if (i < _saveSelectButtons.Count && i < _saveDeleteButtons.Count)
                {
                    SetSlotButtons(_saveSelectButtons[i], _saveDeleteButtons[i], saveSlots[i]);
                }
            }
        }

        private void SetSlotButtons(Button saveButton, Button deleteButton,SaveSlotData slotData)
        {
            SetButton(saveButton, slotData);
            SetDeleteButton(deleteButton, slotData);
        }

        private void SetButton(Button button, SaveSlotData slotData)
        {
            button.text = slotData.SaveSlotName;
            button.style.width = new StyleLength(Length.Percent(75));
        }

        private void SetDeleteButton(Button button, SaveSlotData slotData)
        {
            button.text = "delete save";
            button.userData = slotData.SaveSlotName;
            button.style.display = DisplayStyle.Flex;
            button.style.width = new StyleLength(Length.Percent(25));
        }
        
        private void CacheButtons()
        {
            _saveSelectButtons = new List<Button>();
            _saveDeleteButtons = new List<Button>();
    
            List<VisualElement> containers = new List<VisualElement>();
    
            foreach (var element in _saveSelectContainer.Children().ToList())
            {
                if (element.ClassListContains("save--container"))
                    containers.Add(element);
            }

            foreach (var container in containers)
            {
                Button saveButton = container.Q<Button>(className: "save--select--button");
                if (saveButton != null)
                    _saveSelectButtons.Add(saveButton);
                
                Button deleteButton = container.Q<Button>(className: "save--delete--button");
                if (deleteButton != null)
                    _saveDeleteButtons.Add(deleteButton);
            }
        }

        private void ClearButtons()
        {
            foreach (var button in _saveSelectButtons)
            {
                button.text = null;
            }
            
            foreach (var button in _saveDeleteButtons)
            {
                button.text = null;
                button.userData = null;
            }
        }
    }
}