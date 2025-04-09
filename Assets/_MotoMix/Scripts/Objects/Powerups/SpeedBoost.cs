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
            vehicle.GetComponent<KartDrivingController>().Boost(boostMagnitude);
        }
    }
}
