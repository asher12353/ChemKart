using UnityEngine;

namespace ChemKart
{
    public class CreateBomb : Powerup
    {
        public float distanceAwayFromVehicle = 5f;
        public Bomb bomb;

        public CreateBomb()
        {
            recipeString = "RR";
        }

        public override void Effect(GameObject vehicle)
        {
            Transform sphere = vehicle.GetComponentInChildren<Sphere>().transform; 
            Vector3 pos = sphere.position;
            Vector3 spawnPosition = pos + vehicle.transform.GetComponentInChildren<MotorcycleModel>().transform.forward * distanceAwayFromVehicle;
            Bomb b = Instantiate(bomb, spawnPosition, Quaternion.identity);
            b.StartExplosion();
        }
    }
}
