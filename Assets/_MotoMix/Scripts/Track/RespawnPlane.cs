using UnityEngine;

namespace ChemKart
{
    public class RespawnPlane : MonoBehaviour
    {
        void OnCollisionEnter(Collision other)
        {
            DrivingPhysics driver = other.transform.parent.GetComponent<DrivingPhysics>();
            driver.Respawn();
        }
    }
}
