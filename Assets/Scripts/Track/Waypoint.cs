using UnityEngine;

namespace ChemKart
{
    public class Waypoint : MonoBehaviour
    {
        public int m_WaypointIndex;
        public Waypoint m_NextWaypoint;
        
        public void OnTriggerEnter(Collider other)
        {
            DrivingPhysics driver = other.transform.parent.GetComponent<DrivingPhysics>();
            if(!driver)
            {
                return;
            }
            driver.SetWaypoint(this);
            //Debug.Log(m_WaypointIndex);
        }
    }
}
