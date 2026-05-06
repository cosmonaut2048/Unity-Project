using System.Collections.Generic;
using System.Linq;
using Gameflow;
using Services;
using Services.SaveSlotComponents;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.MenuScreen
{
    public class SaveSelectController : MonoBehaviour
    {
        private VisualElement _saveSelectContainer;
        private List<Button> _saveSelectButtons;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            _saveSelectContainer = root.Q<VisualElement>("Save_Select_Container");
            
            // Кешируем кнопки.
            CacheSaveSelectButtons();
            
            // Очищаем экран.
            ClearButtons();
            
            // Расставляем элементы.
            SetButtons();

            // Подписываемся на события.
            foreach (var button in _saveSelectButtons)
            {
                button.RegisterCallback<ClickEvent>(_ => OnSaveSelectClick(button));
            }
        }

        private void OnSaveSelectClick(Button button)
        {
            string buttonText = button.text;

            if (buttonText == "New Game")
            {
                int buttonIndex = _saveSelectButtons.IndexOf(button);
                string slotName = $"Game{buttonIndex + 1}";
                
                SaveSlotService.Instance.CreateNewSlot(slotName);
                
                SaveSlotService.Instance.SelectSlot(slotName);
                
                SceneController.Instance.LoadScene(nameof(Scenes.HiringScene));
                return;
            }
            
            LoadService.Instance.LoadGameFromSlot(buttonText);
            SceneController.Instance.LoadScene(nameof(Scenes.HallsScene));
        }

        private void SetButtons()
        {
            if (SaveSlotService.Instance.SlotsData.SaveSlots.Count > _saveSelectButtons.Count)
                Debug.Log("More save slots than save select buttons.");

            foreach (var button in _saveSelectButtons)
            {
                button.text = "New Game";
            }
            
            for (int i = 0; i < SaveSlotService.Instance.SlotsData.SaveSlots.Count; i++)
            {
                SetButton(_saveSelectButtons[i], SaveSlotService.Instance.SlotsData.SaveSlots[i]);
            }
        }

        private void SetButton(Button button, SaveSlotData slotData)
        {
            button.text = slotData.SaveSlotName;
        }

        private void CacheSaveSelectButtons()
        {
            List<Button> buttons = new List<Button>();
            
            foreach (var element in _saveSelectContainer.Children().ToList())
            {
                if (element.ClassListContains("save--select--button"))
                    buttons.Add(element as Button);
            }
            
            _saveSelectButtons = buttons;
        }

        private void ClearButtons()
        {
            foreach (var button in _saveSelectButtons)
            {
                button.text = null;
            }
        }
    }
}