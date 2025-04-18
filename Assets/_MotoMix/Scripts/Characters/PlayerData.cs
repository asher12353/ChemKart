using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

namespace ChemKart
{
    public class PlayerData
    {
        public TMP_InputField m_Name;
        public InputDevice m_InputDevice;

        public PlayerData(InputDevice inputDevice, TMP_InputField inputField)
        {
            m_Name = inputField;
            m_InputDevice = inputDevice;
        }
    }
}
