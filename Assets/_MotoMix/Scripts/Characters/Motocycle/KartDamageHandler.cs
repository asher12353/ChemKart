using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;

namespace ChemKart
{
    public class KartDamageHandler : MonoBehaviour
    {
        private KartController controller;
        [SerializeField] private float respawnTime = 3.0f;
        public bool IsDamaged { get; private set; }
        public bool CanDamage { get; set; }
        public bool IsShielded { get; set; }

        private void Start()
        {
            controller = GetComponent<KartController>();
        }

        public async void ApplyDamage()
        {
            if (IsShielded)
            {
                IsShielded = false;
                return;
            }

            controller.CurrentSpeed = 0;
            IsDamaged = true;
            await UniTask.Delay(TimeSpan.FromSeconds(respawnTime));
            IsDamaged = false;
        }

        public void Respawn()
        {
            controller.Sphere.position = controller.CurrentWaypoint.transform.position;
            controller.CurrentSpeed = 0;
        }
    }
}
