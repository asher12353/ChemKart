using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

namespace ChemKart
{
    public class PlayerJoining : MonoBehaviour
    {
        public static List<PlayerData> playersData = new List<PlayerData>();

        [SerializeField]
        private GameObject m_Names;

        private int m_NumPlayersJoined;

        private HashSet<InputDevice> m_JoinedPlayersControllerID;


        private void Awake()
        {
            m_NumPlayersJoined = 0;
            m_JoinedPlayersControllerID = new HashSet<InputDevice>();
            foreach(TMP_InputField name in m_Names.transform.GetComponentsInChildren<TMP_InputField>())
            {
                name.gameObject.SetActive(false);
            }
        }
        
        public void OnPlayerJoined(InputAction.CallbackContext context)
        {
            if(m_NumPlayersJoined >= m_Names.transform.childCount || !gameObject.active || !context.performed || m_JoinedPlayersControllerID.Contains(context.control.device)) return;

            TMP_InputField name = m_Names.transform.GetChild(m_NumPlayersJoined++).GetComponent<TMP_InputField>();
            name.gameObject.SetActive(true);
            m_JoinedPlayersControllerID.Add(context.control.device);
            PlayerData data = new PlayerData(context.control.device, name);
            playersData.Add(data);
        }
    }
}
