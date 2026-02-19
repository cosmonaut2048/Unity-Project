using UnityEngine;

namespace Content
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Content/Item")]
    public class OfficeItem : ScriptableObject
    {
        [Header("Basic Info")] 
        public string itemName;
        public string itemDescription;
        public Sprite itemSprite;
        
        public int maxItemAmount;
        public int itemRarity;
    }
}