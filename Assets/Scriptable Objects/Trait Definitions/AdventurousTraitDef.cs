using Content;
using UnityEngine;

namespace Scriptable_Objects.Trait_Definitions
{
    [CreateAssetMenu(fileName = "AdventurousTraitDef", menuName = "Scriptable Objects/Traits/AdventurousTraitDef")]
    public class AdventurousTraitDef : TraitDef
    {
        // Когда на задании, продуктивность замораживается.
        // Преданность замораживается, а по окончании задания восстанавливается до 100 если была ниже.
    }
}
