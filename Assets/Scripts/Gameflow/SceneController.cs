using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameflow
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        public static SceneController Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                if (loadingScreen)
                    loadingScreen.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            LoadScene(nameof(Scenes.MainMenuScene));
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        
        // private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
        // {
        //     if (loadingScreen)
        //         loadingScreen.SetActive(true);
        //
        //     var currentScene = SceneManager.GetActiveScene();
        //
        //     yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //     
        //     var newScene = SceneManager.GetSceneByName(sceneName);
        //     SceneManager.SetActiveScene(newScene);
        //     
        //     yield return SceneManager.UnloadSceneAsync(currentScene);
        //
        //     yield return null;
        //
        //     if (loadingScreen)
        //         loadingScreen.SetActive(false);
        // }
        
        private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
        {
            if (loadingScreen)
                loadingScreen.SetActive(true);
            
            yield return SceneManager.LoadSceneAsync(sceneName);
            
            yield return null;
            
            if (loadingScreen)
                loadingScreen.SetActive(false);
        }
    }
}
