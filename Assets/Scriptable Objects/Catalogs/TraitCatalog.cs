using System.Collections.Generic;
using Content;
using UnityEngine;

namespace Scriptable_Objects.Catalogs
{
    [CreateAssetMenu(fileName = "TraitsCatalog", menuName = "Scriptable Objects/Catalogs/TraitsCatalog")]
    public class TraitCatalog : ScriptableObject
    {
        [SerializeField] private List<TraitDef> allTraits;
        
        // Доступ по названию.
        private Dictionary<string, TraitDef> _traitsByName;
        // Readonly property.
        public IReadOnlyList<TraitDef> AllTraits => allTraits;

        private void OnEnable()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            _traitsByName = new Dictionary<string, TraitDef>();
            foreach (var trait in allTraits)
            {
                if (trait != null)
                    _traitsByName.TryAdd(trait.traitName, trait);
            }
        }

        public TraitDef GetTraitByName(string traitName)
        {
            if (_traitsByName == null)
                InitializeDictionary();
            
            return _traitsByName.GetValueOrDefault(traitName);
        }

        public TraitDef GetRandomTrait()
        {
            if (allTraits.Count == 0)
                return null;
            
            return allTraits[Random.Range(0, allTraits.Count)];
        }
        
        #if UNITY_EDITOR
        public void RefreshCatalog()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:trait ");
            allTraits = new List<TraitDef>();

            foreach (var guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var trait = UnityEditor.AssetDatabase.LoadAssetAtPath<TraitDef>(path);
                
                if (trait)
                    allTraits.Add(trait);
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }
}
