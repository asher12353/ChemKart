using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChemKart
{
    public class PlayerHUD : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] private List<Image> m_ItemSlots = new();
        [SerializeField] public Sprite emptySprite;
        
        private void Awake()
        {
            if (m_ItemSlots.Count != Inventory.MaxItems)
            {
                Debug.LogWarning($"{name}: Player HUD item slots count does not match inventory max items ({Inventory.MaxItems}).");
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

        public void RemoveItemAtIndex(int index)
        {   
            Image slot = index switch
            {
                0 => m_ItemSlots[0],
                1 => m_ItemSlots[1],
                2 => m_ItemSlots[2],
                _ => null
            };
            
            if (slot)
            {
                slot.sprite = emptySprite;
                ShiftSlots(index);
            }
            else
            {
                Debug.LogWarning($"{name}: Attempted to remove sprite to invalid index {index}.");
            }
        }

        public void RemoveItemAtIndexWithoutShifting(int index)
        {   
            Image slot = index switch
            {
                0 => m_ItemSlots[0],
                1 => m_ItemSlots[1],
                2 => m_ItemSlots[2],
                _ => null
            };
            
            if (slot)
            {
                slot.sprite = emptySprite;
            }
            else
            {
                Debug.LogWarning($"{name}: Attempted to remove sprite to invalid index {index}.");
            }
        }


        void ShiftSlots(int index)
        {
            for(int i = index + 1; i < m_ItemSlots.Count; i++)
            {
                m_ItemSlots[i - 1].sprite = m_ItemSlots[i].sprite;
                m_ItemSlots[i].sprite = emptySprite;
            }
        }
    }
}
