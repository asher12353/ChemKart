using UnityEngine;

namespace ChemKart
{
    public class RespawnPlane : MonoBehaviour
    {
        void OnCollisionEnter(Collision other)
        {
            Debug.Log("Respawning a driver");
            DrivingPhysics driver = other.transform.parent.GetComponent<DrivingPhysics>();
            driver.Respawn();
        }
    }
}
