using System;
using System.IO;
using System.Linq;
using Services.SaveComponents;
using Services.SaveSlotComponents;
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
        
        private GameData LoadData(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Debug.Log($"Game File Loaded: {path}");
                return JsonUtility.FromJson<GameData>(json);
            }
            
            return null;
        }
        
        public void LoadGame()
        {
            SaveSlotData activeSlot = SaveSlotService.Instance.GetActiveSlot();
            
            if (activeSlot == null)
            {
                Debug.LogError("No active save slot!");
                return;
            }
            
            string lastSavePath = activeSlot.GetLastSavePath();
            
            if (string.IsNullOrEmpty(lastSavePath))
            {
                Debug.Log("No saves found in active slot.");
                return;
            }
            
            LoadGameFromPath(lastSavePath);
        }
        
        public void LoadGameFromSlot(string slotName)
        {
            SaveSlotService.Instance.SelectSlot(slotName);
            LoadGame();
        }
        
        public void LoadGameFromPath(string path)
        {
            DataSetter dataSetter = new DataSetter();
            GameData data = LoadData(path);

            if (data == null)
            {
                Debug.Log($"Game Data is null. Path: {path}");
                return;
            }
            
            dataSetter.SetOfficeFromGameData(data);
        }
        
        public string[] GetActiveSlotSaves()
        {
            SaveSlotData activeSlot = SaveSlotService.Instance.GetActiveSlot();
            return activeSlot?.GetAllSavePaths() ?? Array.Empty<string>();
        }
        
        public string[] GetSlotSaves(string slotName)
        {
            var slotsData = AllSaveSlotsData.LoadFromFile();
            var slot = slotsData.SaveSlots.FirstOrDefault(s => s.SaveSlotName == slotName);
            return slot?.GetAllSavePaths() ?? Array.Empty<string>();
        }
    }
}