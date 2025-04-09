using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace ChemKart
{
    public class TempShield : Powerup
    {
        public float m_ShieldTimer = 10f;
        public override void Effect(GameObject vehicle)
        {
            Shield(vehicle);
        }

        async void Shield(GameObject vehicle)
        {
            if(vehicle)
            {
                KartDamageHandler driver = vehicle.transform.GetComponent<KartDamageHandler>();
                driver.IsShielded = true;
                await UniTask.Delay(TimeSpan.FromSeconds(m_ShieldTimer));
                driver.IsShielded = false;
            }
        }
    }
}
