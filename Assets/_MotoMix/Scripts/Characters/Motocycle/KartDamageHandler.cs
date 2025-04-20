using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

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
            Transform waypoint = controller.CurrentWaypoint.transform;

            Vector3 basePosition = waypoint.position;
            Vector3 forward = waypoint.forward;
            Vector3 right = Vector3.Cross(Vector3.up, forward); // Perpendicular to forward

            float rayHeight = 50f;
            float searchWidth = waypoint.localScale.x; // Width of the track segment
            int numSteps = 10;
            int trackLayerMask = LayerMask.GetMask("Track");

            List<Vector3> hitPoints = new List<Vector3>();

            for (int i = 0; i <= numSteps; i++)
            {
                float t = (float)i / numSteps; // from 0 to 1
                float offset = (t - 0.5f) * searchWidth; // from -width/2 to +width/2

                Vector3 origin = basePosition + right * offset + Vector3.up * rayHeight;
                if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayHeight * 2f, trackLayerMask))
                {
                    hitPoints.Add(hit.point);
                }
            }

            if (hitPoints.Count > 0)
            {
                // Average all hit points
                Vector3 sum = Vector3.zero;
                foreach (var point in hitPoints)
                    sum += point;

                Vector3 average = sum / hitPoints.Count;
                return average + Vector3.up;
            }
            controller.CurrentWaypoint = controller.CurrentWaypoint.nextWaypoint;
            // Nothing hit: fallback to base
            return GetSafeRespawnPosition();
        }
    }
}
