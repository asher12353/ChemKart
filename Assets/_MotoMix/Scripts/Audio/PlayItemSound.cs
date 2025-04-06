using UnityEngine;

namespace ChemKart
{
    public class PlayItemSound : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Motorcycle"))
            {
                AkUnitySoundEngine.PostEvent("ItemPickup", gameObject);
            }
        }
    }
}