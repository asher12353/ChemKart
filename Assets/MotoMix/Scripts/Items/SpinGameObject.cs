using UnityEngine;

namespace ChemKart
{
    public class SpinGameObject : MonoBehaviour
    {
        static private float m_SpinValue = 30f;
        static private float m_CurrentYRotation;
        static private bool m_HasBeenUpdatedThisFrame;

        void Update()
        {
            if(!m_HasBeenUpdatedThisFrame)
            {
                m_CurrentYRotation = (m_CurrentYRotation + m_SpinValue * Time.deltaTime) % 360;
                m_HasBeenUpdatedThisFrame = true;
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, m_CurrentYRotation, transform.eulerAngles.z);
        }

        void LateUpdate()
        {
            m_HasBeenUpdatedThisFrame = false;
        }
    }
}
