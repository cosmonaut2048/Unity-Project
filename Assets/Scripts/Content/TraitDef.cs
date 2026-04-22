using Runtime;
using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "TraitDef", menuName = "Scriptable Objects/Content/TraitDef")]
    public class TraitDef : ScriptableObject
    {
        [Header("Trait Info")]
        public string traitName;
        public string traitDescription;
        public string modDescription; // Короткое описание (напр. "+1 к социальным навыкам"). Возможно добавить выделение цветом в UI (нужно создать отдельный класс).

        /* ------------------------------ Модификаторы тиков ------------------------------ */
        public virtual bool IsUniqueProductivityTick() => false;
        public virtual bool IsUniqueLoyaltyTick() => false;
        public virtual int ProductivityTickSize(WorkerRuntime workerRuntime) => workerRuntime.Worker.BaseProductivityTickSize;
        public virtual int LoyaltyTickSize(WorkerRuntime workerRuntime) => workerRuntime.Worker.BaseLoyaltyTickSize;

        /* ------------------------------ Модификаторы в начале дня ------------------------------ */
        public virtual int OnStartOfDayProductivity(int baseProductivity) => baseProductivity;
        public virtual void OnStartOfDay(WorkerRuntime workerRuntime) { }
        
        /* ------------------------------ Модификаторы в конце дня ------------------------------ */
        
        public virtual int OnEndOfDayLoyalty(int baseLoyalty) => baseLoyalty;
        public virtual void OnEndOfDay(WorkerRuntime worker) { }
        
        /* ------------------------------ Модификаторы кофе ------------------------------ */
        public virtual void OnCoffee(WorkerRuntime worker) { }
        public virtual int OnCoffeeProductivity(int baseProductivity) => baseProductivity;
        public virtual int OnCoffeeLoyalty(int baseLoyalty) => baseLoyalty;
        
        /* ------------------------------ Модификаторы перерывов ------------------------------ */
        public virtual void OnBreak(WorkerRuntime worker) { }
        
        /* ------------------------------ Модификаторы заданий ------------------------------ */
        // Заморозки.
        public virtual bool OnTaskFreezeProductivity(WorkerRuntime worker) => false;
        public virtual bool OnTaskFreezeLoyalty(WorkerRuntime worker) => false;
        // Модификаторы.
        public virtual int OnTaskLoyaltyConditional(int baseLoyalty, TaskRuntime task) => baseLoyalty;
        public virtual int OnTaskProductivityConditional(int baseProductivity, TaskRuntime task) => baseProductivity;
        public virtual int OnTaskProductivity(int baseProductivity) => baseProductivity;
        public virtual int OnTaskLoyalty(int baseLoyalty) => baseLoyalty;
        
        /* ------------------------------ Модификаторы навыков ------------------------------ */
        // Пассивные модификаторы навыков.
        public virtual int ModifyPatience(int baseValue) => baseValue;
        public virtual int ModifySocial(int baseValue) => baseValue;
        public virtual int ModifyIntellectual(int baseValue) => baseValue;
        public virtual int ModifyPhysical(int baseValue) => baseValue;
        
        // Модификаторы навыков по условию.
        public virtual int ModifyPatienceConditional(int baseValue, TaskRuntime task) => baseValue;
        public virtual int ModifySocialConditional(int baseValue, TaskRuntime task) => baseValue;
        public virtual int ModifyIntellectualConditional(int baseValue, TaskRuntime task) => baseValue;
        public virtual int ModifyPhysicalConditional(int baseValue, TaskRuntime task) => baseValue;
    }
}
