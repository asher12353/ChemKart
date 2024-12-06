using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
namespace ChemKart
{
    public class ChemicalCrafter : MonoBehaviour
    {
        public Powerup[] recipes;
        public int maxPowerups = 2;

        List<Powerup> possibleRecipes = new();
        Powerup powerupToBeCrafted;
        Inventory inventory;

        List<Collectable> items;
        List<Powerup> powerups = new();

        void Start()
        {
            inventory = transform.GetChild(0).GetComponent<Inventory>();
            inventory.InventoryChanged += CheckAllPossibleRecipes;
        }

        public void CraftEvent(InputAction.CallbackContext context)
        {
            if(powerupToBeCrafted == null)
            {
                return;
            }
            string recipeString = powerupToBeCrafted.recipeString;
            for(int i = 0; i < items.Count; i++)
            {
                for(int j = i + 1; j < items.Count; j++)
                {
                    if(TwoChemicalItemIsCraftable(i, j, recipeString))
                    {
                        CraftPowerup(powerupToBeCrafted, i, j, -1);
                        return;
                    }
                    for(int k = j + 1; k < items.Count; k++)
                    {
                        if(ThreeChemicalItemIsCraftable(i, j, k, recipeString))
                        {
                            CraftPowerup(powerupToBeCrafted, i, j, k);
                            return;
                        }
                    }
                }
            }
        }

        public void UseItemEvent(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                Vector2 keyValue = context.ReadValue<Vector2>();
                if(powerups.Count > 0 && keyValue.y == 1f)
                {
                    powerups[0].Effect(this.gameObject);
                    powerups.Remove(powerups[0]);
                }
            }
            else
            {

            }
        }

        void CheckAllPossibleRecipes()
        {
            possibleRecipes = new();
            items = inventory.M_Items();
            for(int i = 0; i < items.Count; i++)
            {
                for(int j = i + 1; j < items.Count; j++)
                {
                    foreach(Powerup recipe in recipes)
                    {
                        if(!possibleRecipes.Contains(recipe) && TwoChemicalItemIsCraftable(i, j, recipe.recipeString))
                        {
                            possibleRecipes.Add(recipe);
                        }
                    }
                    for(int k = j + 1; k < items.Count; k++)
                    {
                        foreach(Powerup recipe in recipes)
                        {
                            if(!possibleRecipes.Contains(recipe) && ThreeChemicalItemIsCraftable(i, j, k, recipe.recipeString))
                            {
                                possibleRecipes.Add(recipe);
                            }
                        }
                    }
                }
            }
            foreach(Powerup possibleRecipe in possibleRecipes)
            {
                Debug.Log("Possible recipe: " + possibleRecipe);
            }
            powerupToBeCrafted = possibleRecipes.Count > 0 ? possibleRecipes[0] : null;
            
        }

        bool PermutationIsPossible(int x, int y, int z, string recipeString) { return ChemVal(x) + ChemVal(y) + ChemVal(z) == recipeString; }

        bool TwoChemicalItemIsCraftable(int i, int j, string recipeString) { return ChemVal(i) + ChemVal(j) == recipeString || ChemVal(j) + ChemVal(i) == recipeString; }

        bool ThreeChemicalItemIsCraftable(int i, int j, int k, string recipeString) { return PermutationIsPossible(i, j, k, recipeString) || PermutationIsPossible(i, k, j, recipeString) || PermutationIsPossible(j, i, k, recipeString) || PermutationIsPossible(j, k, i, recipeString) || PermutationIsPossible(k, i, j, recipeString) || PermutationIsPossible(k, j, i, recipeString); }

        string ChemVal(int n) { return ((Chemical)items[n]).chemicalValue; }

        void CraftPowerup(Powerup powerup, int i, int j, int k)
        {
            if(powerup == null)
            {
                return;
            }
            Chemical item1 = (Chemical)items[i], item2 = (Chemical)items[j], item3 = null;
            if(k >= 0)
            {
                item3 = (Chemical)items[k];
            }
            powerups.Add(powerup);
            Debug.Log("Added powerup: " + powerup.name);
            inventory.RemoveItem(item1);
            inventory.RemoveItem(item2);
            inventory.RemoveItem(item3);
        }
    }
}
