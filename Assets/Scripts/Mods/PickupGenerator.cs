using UnityEngine;
using System.Collections.Generic;

namespace ChemKart
{
    public class PickupGenerator : MonoBehaviour
    {
        public GameObject pickupTransforms;
        public GameObject pickupPrefab;

        public void GeneratePickups()
        {
            if(pickupTransforms == null)
            {
                Debug.LogError("Can't find the pickup parent, aborting");
                return;
            }
            List<GameObject> pickups = new List<GameObject>();
            foreach(Transform child in pickupTransforms.transform)
            {
                GameObject pickup = Instantiate(pickupPrefab, child.position, child.rotation);
                pickups.Add(pickup);
            }

            foreach(GameObject pickup in pickups)
            {
                pickup.transform.SetParent(pickupTransforms.transform);
            }
        }
    }
}
