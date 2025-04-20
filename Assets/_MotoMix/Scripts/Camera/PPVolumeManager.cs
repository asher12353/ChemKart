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
        private Bloom bloom;

        void Start()
        {
            if (volume && volume.profile)
            {
                volume.profile = Instantiate(volume.profile);
                volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
                volume.profile.TryGet<LensDistortion>(out lensDistortion);
                volume.profile.TryGet<Bloom>(out bloom);

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
            float speed = controller.CurrentSpeed;

            // Chromatic aberration: increase with speed
            chromaticAberration.intensity.value = Mathf.Clamp01(speed / 50f);

            // Lens distortion: stretch the screen subtly
            lensDistortion.intensity.value = Mathf.Lerp(-10, -40, speed / 50f);

            // Bloom: boost glow at high speeds
            bloom.intensity.value = Mathf.Lerp(3f, 10f, speed / 50f);
        }
    }
}
