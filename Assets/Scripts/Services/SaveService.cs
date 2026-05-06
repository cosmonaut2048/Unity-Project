using System.IO;
using Services.SaveComponents;
using Services.SaveSlotComponents;
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
                savePath = Path.Combine(Application.persistentDataPath, "GameSave.json");
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void SaveGame()
        {
            savePath = SaveSlotService.Instance?.GetSavePath();
            
            if (string.IsNullOrEmpty(savePath))
            {
                Debug.LogError("Cannot save: invalid path!");
                return;
            }
            
            // Если нет папки, создаём.
            string directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
                if (directory != null)
                    Directory.CreateDirectory(directory);
            
            GameData data = new GameData();
            data.SetGameData();
            
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            
            Debug.Log($"Game Saved to: {savePath}");
            
            // Обновляем файл слотов.
            AllSaveSlotsData.LoadFromFile()?.SaveToFile();
        }
    }
}
