using UnityEngine;

namespace ChemKart
{
    public class Sphere : MonoBehaviour
    {
        private KartController controller;

        private void Start()
        {
            controller = GetComponentInParent<KartController>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Motorcycle")) // if it's a motorcycle we hit
            {
                if (controller != null && !controller.DamageHandler.CanDamage)
                {
                    KartDamageHandler otherController = collision.gameObject.GetComponentInParent<KartDamageHandler>();
                    if (otherController != null && otherController != controller)
                    {
                        if (!otherController.IsDamaged && !otherController.IsShielded)
                        {
                            otherController.ApplyDamage();
                        }
                        else if (otherController.IsShielded)
                        {
                            otherController.IsShielded = false;
                        }
                    }
                }
            }
        }
    }
}
