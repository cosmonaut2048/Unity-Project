using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameflow
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene(nameof(Scenes.CoreScene), LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnCoreSceneLoaded;
        }
    
        private void OnCoreSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == nameof(Scenes.CoreScene))
            {
                SceneManager.sceneLoaded -= OnCoreSceneLoaded;
                SceneManager.UnloadSceneAsync(gameObject.scene);
            }
        }
    }
}