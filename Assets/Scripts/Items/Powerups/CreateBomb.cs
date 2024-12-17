using UnityEngine;

namespace ChemKart
{
    public class CreateBomb : Powerup
    {
        public float distanceFromVehicle = 5f;
        public Bomb bomb;

        public CreateBomb()
        {
            recipeString = "RR";
        }

        public override void Effect(GameObject vehicle)
        {
            Vector3 pos = vehicle.transform.GetChild(0).transform.position;
            Bomb b = Instantiate(bomb, new Vector3(pos.x, bomb.GetComponent<SphereCollider>().radius + 0.5f, pos.z - distanceFromVehicle), Quaternion.identity);
            b.StartExplosion();
        }
    }
}
