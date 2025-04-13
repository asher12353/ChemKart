using UnityEngine;
using System;

namespace ChemKart
{
    public class Waypoint : MonoBehaviour
    {
        public int waypointIndex;
        public Waypoint nextWaypoint;
        public event Action<Collider> OnTriggerEnterEvent;
        public bool isRequiredWaypoint;
        
        public void OnTriggerEnter(Collider other)
        {
            KartController driver = other.transform.parent.GetComponent<KartController>();
            if(!driver)
            {
                return;
            }
            if(isRequiredWaypoint)
            {
                driver.PassedRequiredWaypoint = true;
            }
            driver.CurrentWaypoint = this;
            OnTriggerEnterEvent?.Invoke(other);
        }
    }
}
