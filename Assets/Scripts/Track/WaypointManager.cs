using UnityEngine;
using System.Collections.Generic;

namespace ChemKart
{
    public class WaypointManager : MonoBehaviour
    {
        public GameObject tracks;
        public static List<Waypoint> m_Waypoints = new();

        void Awake()
        {
            int i = 0;
            Waypoint previousWaypoint = null;
            foreach(Transform child in tracks.transform)
            {
                foreach(Transform waypointObject in child)
                {
                    if(!waypointObject.gameObject.activeSelf)
                    {
                        continue;
                    }
                    Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
                    if(i != 0)
                    {
                        previousWaypoint.m_NextWaypoint = waypoint;
                    }
                    previousWaypoint = waypoint;
                    waypoint.m_WaypointIndex = i++;
                    m_Waypoints.Add(waypoint);
                }
            }
            m_Waypoints[i - 1].m_NextWaypoint = m_Waypoints[0];
        }
    }
}
