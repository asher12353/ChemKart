using System.Collections;
using System.Threading.Tasks; 
using UnityEngine;

namespace ChemKart
{
    public class Rainbow : Powerup
    {
        // 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public override void Effect(GameObject vehicle)
        {
            RunRainbowEffect(vehicle);
        }
        private async void RunRainbowEffect(GameObject vehicle)
        {
            await RainbowEffect(vehicle);
        }

        private async Task RainbowEffect(GameObject vehicle)
        {
            DrivingPhysics physics = vehicle.GetComponent<DrivingPhysics>();
            Renderer[] renderers = vehicle.GetComponentsInChildren<Renderer>();

            if (physics != null && renderers.Length > 0)
            {
                // Store the original colors
                Color[] originalColors = new Color[renderers.Length];
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i].material.HasProperty("_Color"))
                    {
                        originalColors[i] = renderers[i].material.color;
                    }
                }

                // Enable power-up effects
                physics.shielded = true;
                physics.canAttack = true;
                physics.accelerationSpeed *= 2;

                // Flash rainbow colors for 5 seconds
                float elapsedTime = 0f;
                while (elapsedTime < 5f)
                {
                    foreach (Renderer renderer in renderers)
                    {
                        if (renderer.material.HasProperty("_Color"))
                        {
                            renderer.material.color = Random.ColorHSV();
                        }
                    }

                    elapsedTime += 0.1f;
                    await Task.Delay(100); // Wait for 0.1 seconds (100ms)
                }

                // Reset to original colors
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i].material.HasProperty("_Color"))
                    {
                        renderers[i].material.color = originalColors[i];
                    }
                }

                // Reset power-up effects
                physics.shielded = false;
                physics.canAttack = false;
                physics.accelerationSpeed /= 2;
            }
        }
    }
}
