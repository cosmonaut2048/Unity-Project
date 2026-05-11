using Content;

namespace Services.SaveComponents
{
    [System.Serializable]
    public class TraitDefData
    {
        public string traitName;
        
        public void SetDataFromTraitDef(TraitDef trait)
        {
            traitName = trait ? trait.name : "";
        }
    }
}