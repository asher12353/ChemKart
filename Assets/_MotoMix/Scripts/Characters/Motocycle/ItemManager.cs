using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace ChemKart
{
    public class ItemManager : MonoBehaviour
    {   
        [SerializeField] private PlayerHUD m_PlayerHUD;
        private Powerup m_Powerup1;
        private Powerup m_Powerup2;

        public void ItemUsedEvent(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                float keyValue = context.ReadValue<float>();
                if(m_Powerup1 && keyValue == -1f)
                {
                    m_Powerup1.Effect(this.gameObject);
                    m_Powerup1 = null;
                    if(m_PlayerHUD)
                    {
                        m_PlayerHUD.RemoveItemAtIndexWithoutShifting(0);
                    }
                }
                if(m_Powerup2 && keyValue == 1f)
                {
                    m_Powerup2.Effect(this.gameObject);
                    m_Powerup2 = null;
                    if(m_PlayerHUD)
                    {
                        m_PlayerHUD.RemoveItemAtIndexWithoutShifting(1);
                    }
                }
            }
        }

        public bool AddPowerup(Powerup powerup)
        {
            if(m_Powerup1 == null)
            {
                m_Powerup1 = powerup;
                if(m_PlayerHUD)
                {
                    m_PlayerHUD.AddItemAtIndex(0, powerup.powerupSprite);
                }
            }
            else if(m_Powerup2 == null)
            {
                m_Powerup2 = powerup;
                if(m_PlayerHUD)
                {
                    m_PlayerHUD.AddItemAtIndex(1, powerup.powerupSprite);
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}