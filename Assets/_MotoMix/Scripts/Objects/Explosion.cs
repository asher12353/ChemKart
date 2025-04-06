using UnityEngine;
using System.Collections;

namespace ChemKart
{
    public class Explosion : MonoBehaviour
    {
        public float explosionRadius = 2f;
        public float explosionSpeed = 0.1f;
        void Awake()
        {
            transform.localScale = Vector3.zero;
            StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            while(transform.localScale.magnitude <= explosionRadius)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(explosionRadius, explosionRadius, explosionRadius), explosionSpeed * Time.deltaTime);
                yield return null;
            }
            Destroy(this.gameObject);
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
