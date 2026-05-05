using System.IO;
using Services.SaveComponents;
using UnityEngine;

namespace Services
{
    public class SaveService : MonoBehaviour
    {
        [SerializeField] private string savePath;
        
        public string SavePath => savePath;
        public static SaveService Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                savePath = Application.persistentDataPath + "/GameSave.json";
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void UpdatePath()
        {
            savePath = Application.persistentDataPath + "/GameSave.json";
        }

        public void SaveGame()
        {
            GameData data = new GameData();
            data.SetGameData();
            
            string json = JsonUtility.ToJson(data, true);
            
            UpdatePath();
            File.WriteAllText(savePath, json);
            
            Debug.Log($"Game Saved to: {savePath}");
        }
    }
}
