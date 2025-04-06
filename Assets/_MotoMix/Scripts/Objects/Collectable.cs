using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace ChemKart
{
    public class Collectable : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Collider m_Collider;
        
        [Header("Collectable")]
        [SerializeField] private Sprite m_InventoryIcon;
        [SerializeField] private float m_RespawnTime = 10f;
        
        public Sprite InventoryIcon => m_InventoryIcon;
        
        private void Awake()
        {
            if (m_Collider)
            {
                m_Collider.isTrigger = true;
            }
            else
            {
                Debug.LogWarning($"{name}: Collectable collider is not set.");
            }

            if (!m_InventoryIcon)
            {
                Debug.LogWarning($"{name}: Collectable inventory icon is not set.");
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Inventory>() is { } inventory)
            {
                if (inventory.AddItem(this))
                {
                    WaitForRespawn().Forget();
                }
            }
        }
        
        private async UniTaskVoid WaitForRespawn()
        {
            gameObject.SetActive(false);
            await UniTask.Delay(TimeSpan.FromSeconds(m_RespawnTime));
            gameObject.SetActive(true);
        }
    }
}
