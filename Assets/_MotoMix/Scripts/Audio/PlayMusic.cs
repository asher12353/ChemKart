using UnityEngine;

namespace ChemKart
{
    public class PlayMusic : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            AkUnitySoundEngine.PostEvent("PlayMusic", gameObject);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
