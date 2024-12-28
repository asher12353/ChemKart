using UnityEngine;
using System;

namespace ChemKart
{
    public class Waypoint : MonoBehaviour
    {
        public int waypointIndex;
        public Waypoint nextWaypoint;
        public event Action<Collider> OnTriggerEnterEvent;
        [SerializeField] private bool m_IsRequiredWaypoint;
        
        public void OnTriggerEnter(Collider other)
        {
            DrivingPhysics driver = other.transform.parent.GetComponent<DrivingPhysics>();
            if(!driver)
            {
                return;
            }
            if(m_IsRequiredWaypoint)
            {
                Debug.Log("Passed a required waypoint");
                driver.passedRequiredWaypoint = true;
            }
            driver.SetWaypoint(this);
            OnTriggerEnterEvent?.Invoke(other);
        }
    }
}
