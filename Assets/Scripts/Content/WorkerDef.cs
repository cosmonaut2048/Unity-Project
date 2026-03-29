using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Content
{
    [CreateAssetMenu(fileName = "Worker", menuName = "Scriptable Objects/Content/WorkerDef")]
    public class WorkerDef : ScriptableObject
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
        
        [Header("Personality traits")]
        [CanBeNull] public List<TraitDef> personalityTraits;

        private readonly int _baseNoBreakThreshold = 3;
        private readonly int _baseLoyaltyTickSize = 5;
        private readonly int _baseProductivityTickSize = 20;
        public int BaseNoBreakThreshold => _baseNoBreakThreshold;
        public int BaseLoyaltyTickSize => _baseLoyaltyTickSize;
        public int BaseProductivityTickSize => _baseProductivityTickSize;
    }
}

