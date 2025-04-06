using UnityEngine;

namespace ChemKart
{
    public class Explosion : MonoBehaviour
    {
        public float explosionRadius = 2f;
        public float explosionSpeed = 0.1f;
        void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            float val = explosionSpeed * Time.deltaTime;
            transform.localScale += new Vector3(val, val, val);
            // I'm just comparing x here since I can't compare two vector3s
            if(transform.localScale.x >= explosionRadius)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            DrivingPhysics driver = other.transform.parent.transform.GetComponent<DrivingPhysics>();
            if(driver)
            {
                driver.Damage();
            }
        }
    }
}
