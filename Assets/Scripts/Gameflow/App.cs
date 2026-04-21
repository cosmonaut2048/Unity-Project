using Runtime;
using UnityEngine;

namespace Gameflow
{
    [CreateAssetMenu(fileName = "App", menuName = "Scriptable Objects/Gameflow/App")]
    public class App : ScriptableObject
    {
        [SerializeField] OfficeRuntime office;
    }
}
