using System.Collections.Generic;
using UnityEngine;
using System;

namespace ChemKart
{
    public class Inventory : MonoBehaviour
    {
        public event Action InventoryChanged;

        public const int MaxItems = 3;
        
        [Header("Character")]
        [SerializeField] private Character m_Owner;
        
        [Header("Inventory")]
        [SerializeField] private List<Collectable> m_Items = new();
        
        private void Awake()
        {
            if (!m_Owner)
            {
                Debug.LogWarning($"{name}: Inventory owner is not set.");
            }
        }
        
        /// <summary>
        /// Attempts to add an item to the inventory.
        /// </summary>
        /// <param name="item">The item being added to the inventory.</param>
        /// <returns>true if the item was added successfully, otherwise false.</returns>
        public bool AddItem(Collectable item)
        {
            if (m_Items.Count >= MaxItems)
            {
                return false;
            }

            m_Items.Add(item);
            InventoryChanged?.Invoke();

            Debug.Log($"{name}: Added {item.name} to {m_Owner.name}'s inventory.");
            
            if (m_Owner is Player { PlayerHUD: not null } player)
            {
                player.PlayerHUD.AddItemAtIndex(m_Items.Count - 1, item.InventoryIcon);
            }
            
            return true;
        }

        public bool RemoveItem(Collectable item)
        {
            if(item == null || !m_Items.Contains(item))
            {
                return false;
            }

            if (m_Owner is Player { PlayerHUD: not null } player)
            {
                player.PlayerHUD.RemoveItemAtIndex(m_Items.IndexOf(item));
            }

            m_Items.Remove(item);
            InventoryChanged?.Invoke();


            return true;
        }

        public List<Collectable> Items() { return m_Items; }
    }
}
