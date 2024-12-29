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
        private GameObject racerObject;
        private List<GameObject> racers;
        private GameObject player; 

        // dictionary for each racer with their associated driving physics 
        private Dictionary<GameObject, DrivingPhysics> drivingPhysics;
        private Dictionary<GameObject, Character> characters;

        public TextMeshProUGUI placementText; // text to update the placement 
        public TextMeshProUGUI totalRacersText; // text to update total racers 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            racerObject = GameObject.Find("Racers"); 
            racers = new List<GameObject>();
            drivingPhysics = new Dictionary<GameObject, DrivingPhysics>(); 
            characters = new Dictionary<GameObject, Character>();
            if( racerObject != null ) 
            {
                // initialize racers list 
                foreach (Transform child in racerObject.transform)
                {
                    if(child.gameObject != null)
                    {
                        racers.Add(child.gameObject);
                        drivingPhysics[child.gameObject] = child.gameObject.GetComponent<DrivingPhysics>();
                        characters[child.gameObject] = child.gameObject.GetComponent<Character>(); 
                        if(characters[child.gameObject] is Player)
                        {
                            player = child.gameObject; 
                        }
                        
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

        // Update is called once per frame
        void Update()
        {
            // Sort racers based on lap and waypoint
            List<GameObject> sortedRacers = new List<GameObject>(racers); // Create a copy to sort

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
                int waypointA = drivingPhysics[racerA].GetWaypoint().waypointIndex;
                int waypointB = drivingPhysics[racerB].GetWaypoint().waypointIndex;

                return waypointB.CompareTo(waypointA); // Descending order of waypoints
            });

            placementText.text = (sortedRacers.IndexOf(player) + 1).ToString(); 
            totalRacersText.text = sortedRacers.Count.ToString();

            
            
        }
    }
}
