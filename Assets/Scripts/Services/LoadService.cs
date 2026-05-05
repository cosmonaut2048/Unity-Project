using System.IO;
using Services.SaveComponents;
using UnityEngine;

namespace Services
{
    public class LoadService : MonoBehaviour
    {
        public static LoadService Instance { get; private set; }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private GameData LoadData()
        {
            if (File.Exists(SaveService.Instance.SavePath))
            {
                string json = File.ReadAllText(SaveService.Instance.SavePath);
                Debug.Log($"Game File Loaded: {SaveService.Instance.SavePath}");
                return JsonUtility.FromJson<GameData>(json);
            }
            
            return null;
        }
        
        public void LoadGame()
        {
            DataSetter dataSetter = new DataSetter();
            
            GameData data = LoadData();

            if (data == null)
            {
                Debug.Log("Game Data is null.");
                return;
            }
            
            dataSetter.SetOfficeFromGameData(data);
        }
    }
}