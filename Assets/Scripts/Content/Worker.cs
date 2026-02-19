using JetBrains.Annotations;
using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "Worker", menuName = "Scriptable Objects/Content/Worker")]
    public sealed class Worker : ScriptableObject
    {
        [Header("Basic Info")]
        public string workerName;
        public Sprite portraitSprite;
        public Sprite fullBodySprite;
        
        [Header("Base skills")]
        public int basePatience;
        public int baseSocial;
        public int baseIntellectual;
        public int basePhysical;
        
        [Header("Traits")]
        [CanBeNull] public TraitDef personalityTrait;
        
        [Header("Modified skills")]
        public int SkillPatience => personalityTrait?.ModifyPatience(basePatience) ?? basePatience;
        public int SkillSocial => personalityTrait?.ModifySocial(baseSocial) ?? baseSocial;
        public int SkillIntellectual => personalityTrait?.ModifyIntellectual(baseIntellectual) ?? baseIntellectual;
        public int SkillPhysical => personalityTrait?.ModifyPhysical(basePhysical) ?? basePhysical;

        [Header("Productivity and loyalty")]
        public int productivity = 100;
        public int loyalty = 100;
        
        [Header("Worker State")]
        public bool isEmployed;
        public bool isBusy;
        public bool hadCoffeeToday;
        public int daysWithoutBreak;
        public int lastBreakDay;
    }
}
