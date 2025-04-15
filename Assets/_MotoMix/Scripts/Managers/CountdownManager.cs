using TMPro;
using UnityEngine;
using System.Collections;
using System;

namespace ChemKart
{
    public class CountdownManager : MonoBehaviour
    {
        public TMP_Text countdownText;
        public float countdownInterval = 1f;
        public AudioClip[] countdownClips; // [0] = 3, [1] = 2, [2] = 1, [3] = Go!
        public AudioSource audioSource;

        public static Action onCountdownFinished;

        private void Start()
        {
            StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            string[] numbers = { "3", "2", "1", "GO!" };

            for (int i = 0; i < numbers.Length; i++)
            {
                countdownText.text = numbers[i];
                //audioSource.PlayOneShot(countdownClips[i]);
                yield return new WaitForSeconds(countdownInterval);
            }

            countdownText.text = "";
            onCountdownFinished?.Invoke();
        }
    }
}
