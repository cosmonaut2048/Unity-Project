using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.DialogueLogic;
using Gameflow;
using Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.MultiScene
{
    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private WorkerRuntime speaker;
        [SerializeField] private DialogueConditions condition;
        [SerializeField] private Scenes scene; // Сцена, на которую возвращаемся.

        private void SetDialogueController()
        {
            speaker = DialogueContext.Instance.Speaker;
            condition = DialogueContext.Instance.Condition;
            scene = DialogueContext.Instance.Scene;
        }
        
        private VisualElement _workerPortrait;
        
        private Label _nameText;
        private Label _dialogueText;
        
        private Button _nextDialogueButton;
        private Button _nextSceneButton;

        void Start()
        {
            SetDialogueController();
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            // Визуальная часть.
            _workerPortrait = root.Q<VisualElement>("worker_portrait");
            // Текст.
            _nameText = root.Q<Label>("name_text");
            _dialogueText = root.Q<Label>("dialogue_text");
            // Кнопки.
            _nextDialogueButton = root.Q<Button>("next_dialogue_button");
            _nextSceneButton = root.Q<Button>("next_scene_button");
            
            // Скрываем элементы.
            _nextDialogueButton.style.display = DisplayStyle.None;
            _nextSceneButton.style.display = DisplayStyle.None;
            
            // Отображаем диалог.
            ShowDialogue();
            
            // Подписываемся на события.
            _nextSceneButton.RegisterCallback<ClickEvent>(_ => SceneController.Instance.LoadScene(scene.ToString()));
        }

        private List<string> CutTextBlock(DialogueNode node)
        {
            string textBlock = node.TextBlock;
            List<string> result = textBlock.Split(node.Divider).ToList();
            
            return result;
        }

        private void ShowDialogue()
        {
            DialogueNode node = GetDialogue();
            
            if (!node) return;

            if (node.PlayOnlyOnce)
                node.SetPlayed();
            
            List<string> lines = CutTextBlock(node);
            
            StartCoroutine(ShowLines(lines));
        }
        
        private IEnumerator ShowLines(List<string> lines)
        {
            _nextDialogueButton.style.display = DisplayStyle.Flex;
            
            foreach (string line in lines)
            {
                DisplayDialogue(line);
                yield return new WaitForButtonClick(_nextDialogueButton);
            }
            
            _nextDialogueButton.style.display = DisplayStyle.None;
            _nextSceneButton.style.display = DisplayStyle.Flex;
        }
        
        private DialogueNode GetDialogue()
        {
            if (!speaker ||  !speaker.Worker.Appearance.WorkerDialogueProfile)
                return null;
            
            DialogueSelector selector = new DialogueSelector();
            int currentDay = OfficeRuntime.Instance?.DayOfTheWeek ?? 0;
            
            return selector.SelectNode(
                speaker.Worker.Appearance.WorkerDialogueProfile, 
                speaker, 
                currentDay,
                condition
            );
        }

        private void DisplayDialogue(string text)
        {
            if (_nameText != null) _nameText.text = speaker.Worker.Appearance.WorkerName ?? "Unknown name";
            if (_workerPortrait != null) _workerPortrait.style.backgroundImage = new StyleBackground(speaker.Worker.Appearance.PortraitSprite);
            if (_dialogueText != null) _dialogueText.text = text;
        }
    }
}