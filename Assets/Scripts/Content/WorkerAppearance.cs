using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "Appearance", menuName = "Scriptable Objects/Content/Appearance")]
    public class WorkerAppearance : ScriptableObject
    {
        [Header("Appearance")]
        [SerializeField] private string workerName;
        [SerializeField] private Sprite portraitSprite;
        [SerializeField] private Sprite fullBodySprite;

        public string WorkerName => workerName;
        public Sprite PortraitSprite => portraitSprite;
        public Sprite FullBodySprite => fullBodySprite;
    }
}