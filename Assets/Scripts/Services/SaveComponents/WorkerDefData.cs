using System.Collections.Generic;
using Content;
using JetBrains.Annotations;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class WorkerDefData
    {
        public WorkerAppearance appearance;
        
        public int basePatience;
        public int baseSocial;
        public int baseIntellectual;
        public int basePhysical;
        
        public List<TraitDef> personalityTraits;

        public void SetDataFromWorkerDef([CanBeNull] WorkerDef worker)
        {
            if (!worker) return;
            
            appearance = worker.Appearance;
            basePatience = worker.BasePatience;
            baseSocial = worker.BaseSocial;
            baseIntellectual = worker.BaseIntellectual;
            basePhysical = worker.BasePhysical;
            personalityTraits = worker.PersonalityTraits;
        }
    }
}