using Services.SaveSlotComponents;
using UnityEngine;

namespace Services
{
    public class SaveSlotService : MonoBehaviour
    {
        [SerializeField] private AllSaveSlotsData slotsData;
        
        public AllSaveSlotsData SlotsData => slotsData;
        
        public static SaveSlotService Instance { get; private set; }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                slotsData = AllSaveSlotsData.LoadFromFile();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public SaveSlotData GetActiveSlot()
        {
            var activeSlot = slotsData.GetActiveSlot();
            if (activeSlot == null)
            {
                Debug.LogError("No active slot set!");
                if (slotsData.SaveSlots.Count > 0)
                    return slotsData.SaveSlots[0];
            }
            return activeSlot;
        }
        
        public string GetSavePath()
        {
            var activeSlot = GetActiveSlot();
            if (activeSlot == null)
            {
                Debug.LogError("Cannot get save path: no active slot!");
                return null;
            }
            
            // Циклически переключаем подсохранения
            activeSlot.CycleToNextSubSave();
            return activeSlot.GetCurrentSavePath();
        }
        
        public void SelectSlot(string slotName)
        {
            slotsData.SetActiveSlot(slotName);
        }
        
        public SaveSlotData CreateNewSlot(string slotName)
        {
            return slotsData.CreateNewSlot(slotName);
        }
        
        public bool DeleteSlot(string slotName)
        {
            return slotsData.RemoveSlot(slotName);
        }
    }
}