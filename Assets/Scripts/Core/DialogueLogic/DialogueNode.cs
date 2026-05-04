using UnityEngine;
using UnityEngine.Serialization;

namespace Core.DialogueLogic
{
    [CreateAssetMenu(fileName = "DialogueNode", menuName = "Scriptable Objects/Core/Dialogue Logic/Dialogue Node")]
    public class DialogueNode : ScriptableObject
    {
        [Header("Main")]
        [SerializeField] private string nodeId;
        [TextArea] [SerializeField] private string textBlock;
        [FormerlySerializedAs("trigger")] [SerializeField] private DialogueConditions condition;
        
        [Header("Conditions")] 
        [SerializeField] private int minDay = 0;
        [SerializeField] private int maxDay = 999;

        [SerializeField] private int minLoyalty = 0;
        [SerializeField] private int maxLoyalty = 999;
        
        [SerializeField] private int minProductivity = 0;
        [SerializeField] private int maxProductivity = 999;
        
        [Header("Selection")]
        [SerializeField] private int priority = 0;
        [SerializeField] private bool playOnlyOnce;

        [SerializeField] private bool wasPlayed;
        [SerializeField] private string divider = "---";
        
        public string NodeId => nodeId;
        public string TextBlock => textBlock;
        public DialogueConditions Condition => condition;
        
        public int MinDay => minDay;
        public int MaxDay => maxDay;
        
        public int MinLoyalty => minLoyalty;
        public int MaxLoyalty => maxLoyalty;
        
        public int MinProductivity => minProductivity;
        public int MaxProductivity => maxProductivity;
        
        public int Priority => priority;
        public bool PlayOnlyOnce => playOnlyOnce;
        
        public bool WasPlayed => wasPlayed;
        public string Divider => divider;

        public void SetPlayed()
        {
            wasPlayed = true;
        }
    }
}