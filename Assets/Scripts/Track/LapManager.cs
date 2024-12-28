using UnityEngine;
using System.Collections.Generic;
using System;

namespace ChemKart
{
    public class LapManager : MonoBehaviour
    {
        [SerializeField] private Waypoint m_LapWaypoint;
        [SerializeField] private int m_NumLaps = 3;
        [SerializeField] private TMPro.TextMeshProUGUI m_NumLapsText;
        [SerializeField] private TMPro.TextMeshProUGUI m_PlayerLapText;
        [SerializeField] private TMPro.TextMeshProUGUI m_GameOverText;
        [SerializeField] private Player m_Player;
        [SerializeField] private GameObject m_Racers;
        
        void Start()
        {
            if(!m_LapWaypoint)
            {
                Debug.LogError("The m_LapWaypoint is not set!");
                return;
            }
            m_LapWaypoint.OnTriggerEnterEvent += WaypointCrossed;
            if(!m_NumLapsText)
            {
                Debug.LogError("The m_NumLapsText is not set!");
                return;
            }
            m_NumLapsText.text = m_NumLaps.ToString();
            if(!m_Player)
            {
                Debug.LogError("The m_Player is not set!");
                return;
            }
            if(!m_NumLapsText)
            {
                Debug.LogError("The m_NumLapsText is not set!");
                return;
            }
            m_PlayerLapText.text = "1";
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
            if(character.lapNumber == m_NumLaps + 1)
            {
                m_GameOverText.gameObject.SetActive(true);
                foreach(Transform racer in m_Racers.transform)
                {
                    racer.GetComponent<DrivingPhysics>().enabled = false;
                }
                return;
            }
            if(character is Player && m_Player.lapNumber != 1)
            {
                m_PlayerLapText.text = m_Player.lapNumber.ToString();
            }
        }
    }
}
