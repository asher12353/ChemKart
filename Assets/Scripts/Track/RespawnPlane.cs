using UnityEngine;

namespace ChemKart
{
    public class RespawnPlane : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Respawning a driver");
            DrivingPhysics driver = other.transform.parent.GetComponent<DrivingPhysics>();
            driver.Respawn();
        }
    }
}
