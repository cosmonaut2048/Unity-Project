using Runtime;
using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "TraitDef", menuName = "Scriptable Objects/Content/TraitDef")]
    public class TraitDef : ScriptableObject
    {
        public string traitName;
        public string traitDescription;
        public string shortInfo; // Короткое описание (напр. "+1 к социальным навыкам"). Возможно добавить выделение цветом в UI (нужно создать отдельный класс).
        
        // Пассивные модификаторы навыков (не зависят от чего-либо, выполняются всегда).
        public virtual int ModifyPatience(int baseValue) => baseValue;
        public virtual int ModifySocial(int baseValue) => baseValue;
        public virtual int ModifyIntellectual(int baseValue) => baseValue;
        public virtual int ModifyPhysical(int baseValue) => baseValue;
        
        // Модификаторы навыков по условию.
        public virtual int ModifyPatienceConditional(int baseValue, TaskRuntime task) => baseValue;
        public virtual int ModifySocialConditional(int baseValue, TaskRuntime task) => baseValue;
        public virtual int ModifyIntellectualConditional(int baseValue, TaskRuntime task) => baseValue;
        public virtual int ModifyPhysicalConditional(int baseValue, TaskRuntime task) => baseValue;

        // Модификаторы продуктивности.
        public virtual int OnMorningCoffeeProductivity(int baseProductivity, bool hadCoffee) => baseProductivity;
        public virtual int OnStartOfDayProductivity(int baseProductivity, bool hadCoffeeToday) => baseProductivity;
        
        // Модификаторы преданности.
        public virtual int OnNoBreakLoyaltyPenalty(int baseLoyalty, int daysWithoutBreak) => baseLoyalty;
        
        // Выполнение заданий.
        public virtual int OnTaskProductivity(int baseProductivity, int teammatesAmount) => baseProductivity;
        public virtual int OnTaskProductivity(int baseProductivity) => baseProductivity;
        public virtual int OnTaskLoyalty(int baseLoyalty, int teammatesAmount) => baseLoyalty;
        public virtual int OnTaskLoyalty(int baseLoyalty) => baseLoyalty;
        
        // Остальное.
        public virtual void OnDayStart(WorkerDef workerDef) { }
        public virtual void OnDayEnd(WorkerDef workerDef) { }
        public virtual void OnBreak(WorkerDef workerDef) { }
        public virtual void OnCoffee(WorkerDef workerDef) { }
    }
}
