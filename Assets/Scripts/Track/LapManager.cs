using UnityEngine;
using System.Collections.Generic;
using System;

namespace ChemKart
{
    public class LapManager : MonoBehaviour
    {
        // yo Asher just a heads up you made this while pretty drunk so be sure to do a review of this later
        // I'm leaving this because I got overwhelmed, basically what I was thinking now is that there needs to be some list of players in order to keep track of the number of players, so that when the game starts all the players lapnumber can be set to 1
        [SerializeField] private Waypoint m_LapWaypoint;
        [SerializeField] private int m_NumLaps = 3;
        private List<int> m_PlayersLapNumber; // bad variable name, but a list of what lap the player is currently on
        
        void Start()
        {
            if(!m_LapWaypoint)
            {
                Debug.LogError("The m_LapWaypoint is not set!");
                return;
            }
            m_LapWaypoint.OnTriggerEnterEvent += WaypointCrossed;
        }
        void WaypointCrossed(Collider other)
        {
            Character character = other.transform.parent.GetComponent<Character>();
            if(!character)
            {
                Debug.LogWarning("Couldn't get the character component when crossing the line");
                return;
            }
            character.lapNumber += 1;
            Debug.Log("Lap number " + character.lapNumber);
            if(character.lapNumber == m_NumLaps)
            {
                Debug.Log("Game is over!");
            }
        }
    }
}
