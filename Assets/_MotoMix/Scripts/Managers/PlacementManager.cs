using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace ChemKart
{
    public class PlacementManager : MonoBehaviour
    {
        private GameObject m_RacerObject;
        private List<GameObject> m_Racers;
        private GameObject m_Player; 

        // dictionary for each racer with their associated driving physics 
        private Dictionary<GameObject, KartController> drivingPhysics;
        private Dictionary<GameObject, Character> characters;

        public TextMeshProUGUI placementText;
        public TextMeshProUGUI totalRacersText;

        void Start()
        {
            m_Player = transform.parent.gameObject;
            m_RacerObject = GameObject.Find("Racers"); 
            m_Racers = new List<GameObject>();
            drivingPhysics = new Dictionary<GameObject, KartController>(); 
            characters = new Dictionary<GameObject, Character>();
            if( m_RacerObject != null ) 
            {
                // initialize racers list 
                foreach (Transform child in m_RacerObject.transform)
                {
                    if(child.gameObject != null)
                    {
                        m_Racers.Add(child.gameObject);
                        drivingPhysics[child.gameObject] = child.gameObject.GetComponent<KartController>();
                        characters[child.gameObject] = child.gameObject.GetComponent<Character>(); 
                    }
                    else
                    {
                        Debug.LogError("Racer Child is null"); 
                    }
                }
            }
            else
            {
                Debug.LogError("Racers not found"); 
            }
        }

        void Update()
        {
            // Sort racers based on lap and waypoint
            List<GameObject> sortedRacers = new List<GameObject>(m_Racers); // Create a copy to sort

            sortedRacers.Sort((racerA, racerB) =>
            {
                int lapA = characters[racerA].lapNumber;
                int lapB = characters[racerB].lapNumber;

                // Compare laps first
                if (lapA != lapB)
                {
                    return lapB.CompareTo(lapA); // Descending order of laps
                }

                // If laps are the same, compare waypoints
                int waypointA = drivingPhysics[racerA].CurrentWaypoint.waypointIndex;
                int waypointB = drivingPhysics[racerB].CurrentWaypoint.waypointIndex;

                return waypointB.CompareTo(waypointA); // Descending order of waypoints
            });

            placementText.text = (sortedRacers.IndexOf(m_Player) + 1).ToString(); 
            totalRacersText.text = sortedRacers.Count.ToString();
        }
    }
}
