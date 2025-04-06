using UnityEngine;
using System.Collections.Generic;

namespace ChemKart
{

    // DEPRECIATED: THIS CLASS IS NOT USED IN THE GAME AS OF 3/11/2025
    public class WaypointManager : MonoBehaviour
    {
        public GameObject tracks;
        public static List<Waypoint> m_Waypoints = new();

        void Awake()
        {
            // this could probably be made to be done in editor, instead of when starting to play, but I'm unsure of how to do in engine code
            GrabActiveAllWaypoints(tracks);
            m_Waypoints[m_Waypoints.Count / 2].isRequiredWaypoint = true;
        }

        /// <summary>
        /// Loops through the track to assign the waypoints to the m_Waypoints variable.
        /// </summary>
        /// <param name="track">The gameobject that holds all of the track pieces and waypoints. Should be in the format tracks->trackpieces->waypoints in terms of hierarchy.</param>
        void GrabActiveAllWaypoints(GameObject track)
        {
            if(!track)
            {
                Debug.LogError("The track gameobject has not been set!");
                return;
            }
            int i = 0;
            Waypoint previousWaypoint = null;
            foreach(Transform child in track.transform)
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
                        previousWaypoint.nextWaypoint = waypoint;
                    }
                    previousWaypoint = waypoint;
                    waypoint.waypointIndex = i++;
                    m_Waypoints.Add(waypoint);
                }
            }
            m_Waypoints[i - 1].nextWaypoint = m_Waypoints[0];
        }
    }
}
