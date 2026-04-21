using UnityEngine;

namespace Gameflow
{
    public class DontDestroyOnLoadObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}