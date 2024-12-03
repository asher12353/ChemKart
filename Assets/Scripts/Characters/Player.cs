using UnityEngine;

namespace ChemKart
{
    public class Player : Character
    {
        public PlayerHUD PlayerHUD { get; private set; }
        
        private void Awake()
        {
            PlayerHUD = GetComponentInChildren<PlayerHUD>();
        }
    }
}
