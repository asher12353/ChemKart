using UnityEngine;

namespace ChemKart
{
    public class RespawnPlane : MonoBehaviour
    {
        void OnCollisionEnter(Collision other)
        {
            KartDamageHandler driver = other.transform.parent.GetComponent<KartDamageHandler>();
            driver.Respawn();
        }
    }
}
