using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; 
using System.Collections.Generic;
using TMPro;

namespace ChemKart
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_RacersParent;
        [SerializeField]
        private GameObject m_PlayerPrefab;
        public List<PlayerData> joinedPlayers;
        public Transform spawnPoints;

        public void SpawnPlayers()
        {
            joinedPlayers = PlayerJoining.playersData;
            if(joinedPlayers == null)
            {
                joinedPlayers = new List<PlayerData>();
                InputDevice defaultDevice = Gamepad.all.Count > 0 ? Gamepad.all[0] : Keyboard.current;
                PlayerData data = new PlayerData(defaultDevice, null);
                joinedPlayers.Add(data);
            }

            for(int i = 0; i < joinedPlayers.Count; i++)
            {
                if(i >= spawnPoints.childCount)
                {
                    Debug.LogWarning("Not enought spawn points for all players!");
                    break;
                }

                GameObject player = Instantiate(m_PlayerPrefab, spawnPoints.GetChild(i).position, spawnPoints.GetChild(i).rotation);
                player.transform.SetParent(m_RacersParent.transform);
                player.GetComponentInChildren<LapManager>().racers = m_RacersParent;
                //player.GetComponent<DrivingPhysics>().playerInput = joinedPlayers[i].m_InputDevice;
                player.GetComponentInChildren<TMP_Text>().text = joinedPlayers[i].m_Name?.text;

                CanvasScaler canvasScaler = player.GetComponentInChildren<CanvasScaler>();
                Camera playerCam = player.GetComponentInChildren<Camera>();

                if (canvasScaler != null && playerCam != null)
                {
                    Rect viewport = playerCam.rect;

                    // Adjust reference resolution based on the viewport size
                    canvasScaler.referenceResolution = new Vector2(
                        1920 * viewport.width,  // Scale width dynamically
                        1080 * viewport.height  // Scale height dynamically
                    );
                }
            }
        }
    }
}