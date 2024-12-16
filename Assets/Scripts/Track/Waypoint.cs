using UnityEngine;
using System;

namespace ChemKart
{
    public class Waypoint : MonoBehaviour
    {
        public int waypointIndex;
        public Waypoint nextWaypoint;
        public event Action<Collider> OnTriggerEnterEvent;
        
        public void OnTriggerEnter(Collider other)
        {
            DrivingPhysics driver = other.transform.parent.GetComponent<DrivingPhysics>();
            if(!driver)
            {
                return;
            }
            driver.SetWaypoint(this);
            OnTriggerEnterEvent?.Invoke(other);
        }
    }
}
