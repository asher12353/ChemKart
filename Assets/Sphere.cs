using UnityEngine;

namespace ChemKart
{
    public class Sphere : MonoBehaviour
    {
        DrivingPhysics physics; 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            physics = GetComponentInParent<DrivingPhysics>(); 
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Motorcycle")
            {
                if(physics != null) 
                {
                    if(physics.canAttack)
                    {
                        collision.gameObject.GetComponent<DrivingPhysics>().Damage(); 
                    }
                }
            }
        }
    }
}
