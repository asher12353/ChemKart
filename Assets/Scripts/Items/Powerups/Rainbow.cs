using System.Collections;
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
            StartCoroutine(RainbowEffect(vehicle)); 
        }

        private IEnumerator RainbowEffect(GameObject vehicle)
        {
            DrivingPhysics physics = vehicle.GetComponent<DrivingPhysics>();
            Renderer[] renderers = vehicle.GetComponentsInChildren<Renderer>();

            if (physics != null)
            {
                // Store original colors
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

                // Flash rainbow colors for 10 seconds
                float elapsedTime = 0f;
                while (elapsedTime < 10f)
                {
                    foreach (Renderer renderer in renderers)
                    {
                        if (renderer.material.HasProperty("_Color"))
                        {
                            renderer.material.color = Random.ColorHSV();
                        }
                    }

                    elapsedTime += 0.1f;
                    yield return new WaitForSeconds(0.1f);
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
