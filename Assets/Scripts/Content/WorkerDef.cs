using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Content
{
    [CreateAssetMenu(fileName = "Worker", menuName = "Scriptable Objects/Content/WorkerDef")]
    public class WorkerDef : ScriptableObject
    {
        [Header("Appearance")]
        [SerializeField] private WorkerAppearance appearance;
        
        [Header("Base skills")]
        [SerializeField] private int basePatience;
        [SerializeField] private int baseSocial;
        [SerializeField] private int baseIntellectual;
        [SerializeField] private int basePhysical;
        
        [Header("Personality traits")]
        [SerializeField] [CanBeNull] private List<TraitDef> personalityTraits;

        private readonly int _baseNoBreakThreshold = 3;
        private readonly int _baseLoyaltyTickSize = 5;
        private readonly int _baseProductivityTickSize = 20;
        
        // Свойства.
        public int BaseNoBreakThreshold => _baseNoBreakThreshold;
        public int BaseLoyaltyTickSize => _baseLoyaltyTickSize;
        public int BaseProductivityTickSize => _baseProductivityTickSize;
        public WorkerAppearance Appearance => appearance;
        public int BasePatience => basePatience;
        public int BaseSocial => baseSocial;
        public int BaseIntellectual => baseIntellectual;
        public int BasePhysical => basePhysical;
        public List<TraitDef> PersonalityTraits => personalityTraits;
        
        // Метод инициализации.
        public void InitializeWorkerDef(WorkerAppearance newAppearance, int patience, int social, int intellectual, int physical, List<TraitDef> traits)
        {
            appearance = newAppearance;
            basePatience = patience;
            baseSocial = social;
            baseIntellectual = intellectual;
            basePhysical = physical;
            personalityTraits = new List<TraitDef>(traits);
        }
    }
}

