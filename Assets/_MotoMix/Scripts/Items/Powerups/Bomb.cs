using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace ChemKart
{
    public class Bomb : MonoBehaviour
    {
        public float m_FuseTimer;
        public GameObject m_ExplosionSphere;

        public async void StartExplosion()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(m_FuseTimer));
            Explode();
        }

        void Explode()
        {
            Instantiate(m_ExplosionSphere, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
