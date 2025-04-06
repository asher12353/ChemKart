using UnityEngine;

namespace ChemKart
{
    public abstract class Powerup : MonoBehaviour
    {
        public string recipeString;
        public Sprite powerupSprite;

        public abstract void Effect(GameObject vehicle);
    }
}
