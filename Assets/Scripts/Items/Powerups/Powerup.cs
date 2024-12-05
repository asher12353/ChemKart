using UnityEngine;

namespace ChemKart
{
    public abstract class Powerup : MonoBehaviour
    {
        public string recipeString;

        public abstract void Effect();
    }
}
