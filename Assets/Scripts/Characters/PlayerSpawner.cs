using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace ChemKart
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_RacersParent;
        [SerializeField]
        private List<Transform> m_SpawnPoints;
        [SerializeField]
        private GameObject m_PlayerPrefab;
        public List<PlayerData> joinedPlayers;

        private void Awake()
        {
            joinedPlayers = PlayerJoining.Instance.playersData;
            SpawnPlayers();
        }

        private void SpawnPlayers()
        {
            for(int i = 0; i < joinedPlayers.Count; i++)
            {
                if(i >= m_SpawnPoints.Count)
                {
                    Debug.LogWarning("Not enought spawn points for all players!");
                    break;
                }

                GameObject player = Instantiate(m_PlayerPrefab, m_SpawnPoints[i].position, m_SpawnPoints[i].rotation);
                player.transform.SetParent(m_RacersParent.transform);
                player.GetComponentInChildren<LapManager>().racers = m_RacersParent;
                player.GetComponent<DrivingPhysics>().playerInput = joinedPlayers[i].m_InputDevice;
                player.GetComponentInChildren<TMP_Text>().text = joinedPlayers[i].m_Name.text;

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
