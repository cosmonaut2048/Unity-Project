using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "TraitDef", menuName = "Scriptable Objects/Content/TraitDef")]
    public class TraitDef : ScriptableObject
    {
        public string traitName;
        public string traitDescription;
        // public string shortInfo;
        // shortInfo - короткое описание (напр. "+1 к социальным навыкам")
        
        // Skill modifiers
        public virtual int ModifyPatience(int baseValue) => baseValue;
        public virtual int ModifySocial(int baseValue) => baseValue;
        public virtual int ModifyIntellectual(int baseValue) => baseValue;
        public virtual int ModifyPhysical(int baseValue) => baseValue;

        // Productivity modifiers
        public virtual int OnMorningCoffee(int baseProductivity, bool hadCoffee) => baseProductivity;
        public virtual int OnStartOfDayProductivity(int baseProductivity, bool hadCoffeeToday) => baseProductivity;
        
        // Loyalty modifiers
        public virtual int OnNoBreakPenalty(int baseLoyalty, int daysWithoutBreak) => baseLoyalty;
        
        // Extra
        public virtual void OnDayStart(Worker worker) { }
        public virtual void OnDayEnd(Worker worker) { }
        public virtual void OnBreak(Worker worker) { }
        public virtual void OnCoffee(Worker worker) { }
        
        // Task-related
        public virtual int OnTaskProductivity(int baseProductivity, int teammatesAmount) => baseProductivity;
        public virtual int OnTaskProductivity(int baseProductivity) => baseProductivity;
        public virtual int OnTaskLoyalty(int baseLoyalty, int teammatesAmount) => baseLoyalty;
        public virtual int OnTaskLoyalty(int baseLoyalty) => baseLoyalty;
    }
}
