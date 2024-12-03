using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace ChemKart
{
    [RequireComponent(typeof(Collider))]
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private float m_RespawnTime = 10f;
        [SerializeField] private Sprite m_Icon;
        
        private Collider m_Collider;
        
        public Sprite Icon => m_Icon;
        
        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
            m_Collider.isTrigger = true;
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
