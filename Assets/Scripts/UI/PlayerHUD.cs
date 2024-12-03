using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChemKart
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private List<Image> m_ItemSlots = new();

        private void Awake()
        {
            if (m_ItemSlots.Count != Inventory.MaxItems)
            {
                Debug.LogWarning($"{name}: Item slots count does not match inventory max items ({Inventory.MaxItems}).");
            }
        }
        
        public void AddItemAtIndex(int index, Sprite sprite)
        {
            if (!sprite)
            {
                Debug.LogWarning($"{name}: Attempted to add null sprite to index {index}.");
                return;
            }
            
            Image slot = index switch
            {
                0 => m_ItemSlots[0],
                1 => m_ItemSlots[1],
                2 => m_ItemSlots[2],
                _ => null
            };
            
            if (slot)
            {
                slot.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"{name}: Attempted to add sprite to invalid index {index}.");
            }
        }
    }
}
