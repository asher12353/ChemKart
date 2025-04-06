using UnityEngine;

namespace ChemKart
{
    public class SpeedBoost : Powerup
    {
        public float boostMagnitude = 2f;

        public SpeedBoost()
        {
            recipeString = "BB";
        }

        public override void Effect(GameObject vehicle)
        {
            DrivingPhysics driver = vehicle.GetComponent<DrivingPhysics>();
            driver.currentSpeed = driver.accelerationSpeed * boostMagnitude;
        }
    }
}
