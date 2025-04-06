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
                DrivingPhysics driver = vehicle.transform.GetComponent<DrivingPhysics>();
                driver.shielded = true;
                await UniTask.Delay(TimeSpan.FromSeconds(m_ShieldTimer));
                driver.shielded = false;
            }
        }
    }
}
