using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Services.SaveSlotComponents
{
    [Serializable]
    public class SaveSlotData
    {
        [SerializeField] private string saveSlotName = "SaveSlot";
        [SerializeField] private int saveSlotIndex;
        [SerializeField] private int maxSubSaves = 3;
        [SerializeField] private int currentSubSaveIndex;
    
        private string BasePath => Path.Combine(Application.persistentDataPath, "SaveSlots");
    
        public string SaveSlotName => saveSlotName;
        public int SaveSlotIndex => saveSlotIndex;
        public int CurrentSubSaveIndex => currentSubSaveIndex;
        public string SlotFolderPath => Path.Combine(BasePath, saveSlotName);
    
        public string GetCurrentSavePath()
        {
            return Path.Combine(SlotFolderPath, $"Save{currentSubSaveIndex}.json");
        }
        
        public void CycleToNextSubSave()
        {
            currentSubSaveIndex++;
            if (currentSubSaveIndex > maxSubSaves)
                currentSubSaveIndex = 1;
        }
        
        public void Initialize(string name, int index, int maxSaves = 3)
        {
            saveSlotName = name;
            saveSlotIndex = index;
            maxSubSaves = maxSaves;
            currentSubSaveIndex = 0;
            
            // Создаем папку для слота.
            Directory.CreateDirectory(SlotFolderPath);
        }
        
        public string GetLastSavePath()
        {
            if (!Directory.Exists(SlotFolderPath))
                return null;
    
            string[] existingSaves = Directory.GetFiles(SlotFolderPath, "Save*.json");
    
            if (existingSaves.Length == 0)
                return null;
            
            return existingSaves.OrderByDescending(File.GetLastWriteTime).First();
        }
        
        public string[] GetAllSavePaths()
        {
            if (!Directory.Exists(SlotFolderPath))
                return Array.Empty<string>();
    
            string[] existingSaves = Directory.GetFiles(SlotFolderPath, "Save*.json");
            return existingSaves.OrderByDescending(File.GetLastWriteTime).ToArray();
        }
    }
}