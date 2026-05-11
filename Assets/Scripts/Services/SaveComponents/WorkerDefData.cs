using System.Collections.Generic;
using Content;
using JetBrains.Annotations;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class WorkerDefData
    {
        public string appearanceName;
        
        public int basePatience;
        public int baseSocial;
        public int baseIntellectual;
        public int basePhysical;
        
        public List<TraitDefData> personalityTraits;

        public void SetDataFromWorkerDef([CanBeNull] WorkerDef worker)
        {
            if (!worker) return;
            
            appearanceName = worker.Appearance ? worker.Appearance.name : "";
            basePatience = worker.BasePatience;
            baseSocial = worker.BaseSocial;
            baseIntellectual = worker.BaseIntellectual;
            basePhysical = worker.BasePhysical;
            
            personalityTraits = new List<TraitDefData>();
            if (worker.PersonalityTraits != null)
            {
                foreach (var trait in worker.PersonalityTraits)
                {
                    var traitData = new TraitDefData();
                    traitData.SetDataFromTraitDef(trait);
                    personalityTraits.Add(traitData);
                }
            }
        }
    }
}