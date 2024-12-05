using UnityEngine;
using System.Collections;
namespace ChemKart
{
    public class ChemicalCrafter : MonoBehaviour
    {
        public GameObject[] recipes;
        string[] possibleRecipes;
        Inventory inventory;

        void Start()
        {
            inventory = transform.GetComponent<Inventory>();
            inventory.InventoryChanged += CheckAllPossibleRecipes;
        }

        void CheckAllPossibleRecipes()
        {
            Debug.Log("bebop checking recipes ..... done!");
        }
    }
}
