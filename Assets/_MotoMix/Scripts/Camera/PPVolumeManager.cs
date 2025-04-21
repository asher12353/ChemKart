using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ChemKart
{
    public class PPVolumeManager : MonoBehaviour
    {
        [SerializeField]
        private Volume volume; // Assign in prefab or via GetComponentInChildren
        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private KartController controller;

        private static int layerIndex = 0;

        private ChromaticAberration chromaticAberration;
        private LensDistortion lensDistortion;
        
        void Start()
        {
            if (volume && volume.profile)
            {
                volume.profile = Instantiate(volume.profile);
                volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
                volume.profile.TryGet<LensDistortion>(out lensDistortion);

                int assignedLayer = LayerMask.NameToLayer("Player" + layerIndex++);

                gameObject.layer = assignedLayer;
                volume.gameObject.layer = assignedLayer;
                playerCamera.gameObject.layer = assignedLayer;

                var camData = playerCamera.GetUniversalAdditionalCameraData();
                camData.volumeLayerMask = (1 << assignedLayer) | camData.volumeLayerMask;
            }
        }

        void Update()
        {
            float speed = controller.Rigidbody.linearVelocity.magnitude;

            // Chromatic aberration: increase with speed
            chromaticAberration.intensity.value = Mathf.Clamp01(speed / controller.DrivingController.accelerationRate);

            // Lens distortion: stretch the screen subtly
            lensDistortion.intensity.value = Mathf.Lerp(-0.1f, -0.65f, speed / controller.DrivingController.accelerationRate );
        }
    }
}
