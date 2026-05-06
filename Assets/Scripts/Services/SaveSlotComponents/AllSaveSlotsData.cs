using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Services.SaveSlotComponents
{
    [Serializable]
    public class AllSaveSlotsData
    {
        [SerializeField] private List<SaveSlotData> saveSlots = new List<SaveSlotData>();
        [SerializeField] private string activeSlotName;
        [SerializeField] private int maxSlots = 3;
        
        public static string DataPath => Path.Combine(Application.persistentDataPath, "AllSaveSlotsData.json");
        
        public List<SaveSlotData> SaveSlots => saveSlots;
        public int SlotCount => saveSlots.Count;
        public int MaxSlots => maxSlots;
        
        public SaveSlotData GetActiveSlot()
        {
            return saveSlots.FirstOrDefault(s => s.SaveSlotName == activeSlotName);
        }
        
        public void SetActiveSlot(string slotName)
        {
            if (saveSlots.Any(s => s.SaveSlotName == slotName))
            {
                activeSlotName = slotName;
                SaveToFile();
            }
            else
            {
                Debug.LogError($"Slot {slotName} not found!");
            }
        }
        
        public bool AddSlot(SaveSlotData slot)
        {
            if (saveSlots.Count >= maxSlots)
            {
                Debug.LogWarning("Max slots reached!");
                return false;
            }
            
            if (saveSlots.Any(s => s.SaveSlotName == slot.SaveSlotName))
            {
                Debug.LogWarning($"Slot {slot.SaveSlotName} already exists!");
                return false;
            }
            
            saveSlots.Add(slot);
            
            // Если это первый слот, делаем его активным.
            if (saveSlots.Count == 1)
                activeSlotName = slot.SaveSlotName;
                
            SaveToFile();
            return true;
        }
        
        public SaveSlotData CreateNewSlot(string name)
        {
            if (saveSlots.Count >= maxSlots)
                return null;
            
            int newIndex = saveSlots.Count + 1;
            var newSlot = new SaveSlotData();
            newSlot.Initialize(name, newIndex);
    
            AddSlot(newSlot);
            return newSlot;
        }
        
        public void SaveToFile()
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(DataPath, json);
        }
        
        public static AllSaveSlotsData LoadFromFile()
        {
            if (!File.Exists(DataPath))
            {
                AllSaveSlotsData newData = new AllSaveSlotsData();
                newData.SaveToFile();
                return newData;
            }
            
            string json = File.ReadAllText(DataPath);
            return JsonUtility.FromJson<AllSaveSlotsData>(json) ?? new AllSaveSlotsData();
        }
    }
}