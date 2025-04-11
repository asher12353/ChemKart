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
            controller.Sphere.position = GetSafeRespawnPosition();
            controller.CurrentSpeed = 0;
        }

        private Vector3 GetSafeRespawnPosition()
        {
            Vector3 basePosition = controller.CurrentWaypoint.transform.position;
            Vector3 forward = controller.CurrentWaypoint.transform.forward;
            Vector3 right = Vector3.Cross(Vector3.up, forward);

            float rayHeight = 50f;
            float lateralOffset = 0f;

            Vector3 leftOrigin = basePosition + (right * -lateralOffset) + Vector3.up * rayHeight;
            Vector3 rightOrigin = basePosition + (right * lateralOffset) + Vector3.up * rayHeight;

            int trackLayerMask = LayerMask.GetMask("Track");

            bool leftHit = Physics.Raycast(leftOrigin, Vector3.down, out RaycastHit leftHitInfo, rayHeight * 2f, trackLayerMask);
            bool rightHit = Physics.Raycast(rightOrigin, Vector3.down, out RaycastHit rightHitInfo, rayHeight * 2f, trackLayerMask);

            if (leftHit && rightHit)
            {
                Vector3 midpointXZ = (leftHitInfo.point + rightHitInfo.point) * 0.5f;
                return midpointXZ;
            }
            else if (leftHit)
            {
                return leftHitInfo.point;
            }
            else if (rightHit)
            {
                return rightHitInfo.point;
            }

            return basePosition;
        }
    }
}
