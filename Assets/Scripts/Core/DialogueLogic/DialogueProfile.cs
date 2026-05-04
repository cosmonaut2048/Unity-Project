using System.Collections.Generic;
using UnityEngine;

namespace Core.DialogueLogic
{
    [CreateAssetMenu(fileName = "DialogueProfile", menuName = "Scriptable Objects/Core/Dialogue Logic/Dialogue Profile")]
    public class DialogueProfile : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> dialogueNodes = new List<DialogueNode>();

        public DialogueNode GetById(string id)
        {
            return dialogueNodes.Find(n => n.NodeId == id);
        }
        
        public List<DialogueNode> DialogueNodes() => dialogueNodes;
    }
}