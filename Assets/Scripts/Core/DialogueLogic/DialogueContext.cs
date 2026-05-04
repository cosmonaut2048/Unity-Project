using Gameflow;
using Runtime;
using UnityEngine;

namespace Core.DialogueLogic
{
    public class DialogueContext : MonoBehaviour
    {
        [SerializeField] private WorkerRuntime speaker;
        [SerializeField] private DialogueConditions condition;
        [SerializeField] private Scenes scene; // Сцена, на которую возвращаемся.
        
        public WorkerRuntime Speaker => speaker;
        public DialogueConditions Condition => condition;
        public Scenes Scene => scene;
        
        public static DialogueContext Instance { get; private set; }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetDialogueContext(WorkerRuntime newSpeaker, DialogueConditions newCondition, Scenes newScene)
        {
            speaker = newSpeaker;
            condition = newCondition;
            scene = newScene;
        }
    }
}