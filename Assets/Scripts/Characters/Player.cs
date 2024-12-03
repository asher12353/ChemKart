using UnityEngine;

namespace ChemKart
{
    public class Player : Character
    {
        [Header("UI")]
        [SerializeField] private PlayerHUD m_PlayerHUD;
        
        public PlayerHUD PlayerHUD => m_PlayerHUD;
        
        private void Awake()
        {
            if (!m_PlayerHUD)
            {
                Debug.LogWarning($"{name}: Player HUD is not set.");
            }
        }
    }
}
