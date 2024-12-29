using UnityEngine;
using System.Collections;

namespace ChemKart
{
    public class AudioClipPlayer : MonoBehaviour
    {
        [Tooltip("This is just for developers to keep track of what audio sound this should be")]
        [SerializeField] private string m_AudioClipName;
        [SerializeField] private AudioClip m_AudioClip;
        [SerializeField] private float m_StartTime;
        [SerializeField] private float m_EndTime;
        
        private AudioSource m_AudioSource;

        void Awake()
        {
            m_AudioSource = transform.GetComponent<AudioSource>();
            if(!m_AudioSource)
            {
                Debug.LogError("Couldn't get the audio source!");
                return;
            }
            if(!m_AudioClip)
            {
                Debug.LogWarning("No Audio clip was assigned!");
            }
            m_AudioSource.clip = m_AudioClip;
        }
    
        public void PlayClip()
        {
            m_AudioSource.time = m_StartTime;
            m_AudioSource.Play();
            StartCoroutine(StopAfterDuration(m_EndTime - m_StartTime));
        }

        IEnumerator StopAfterDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            m_AudioSource.Stop();
        }
    }
}
