using UnityEngine;

namespace ChemKart
{
    public class PlayItemSound : MonoBehaviour
    {
        public Collectable Collectable { get; private set; }
        public DrivingPhysics drivingPhysics { get; private set; }
        public LapManager LapManager { get; private set; }
        private readonly Player Player;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                AkUnitySoundEngine.PostEvent("ItemSound", gameObject);
            }
        }
    }
}