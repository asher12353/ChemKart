using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
namespace ChemKart
{
    public class ChemicalCrafter : MonoBehaviour
    {
        [SerializeField] private PlayerHUD playerHUD;
        [SerializeField] private Powerup[] recipes;
        List<Powerup> possibleRecipes = new();
        Powerup powerupToBeCrafted;
        Inventory playerInventory;

        List<Collectable> items;
        Powerup powerup1;
        Powerup powerup2;


        void Start()
        {
            playerInventory = transform.GetChild(0).GetComponent<Inventory>();
            playerInventory.InventoryChanged += CheckAllPossibleRecipes;
        }

        public void ItemEvent(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                float keyValue = context.ReadValue<float>();
                if(powerup1 && keyValue == -1f)
                {
                    powerup1.Effect(this.gameObject);
                    powerup1 = null;
                    playerHUD.RemoveItemAtIndexWithoutShifting(0);
                }
                if(powerup2 && keyValue == 1f)
                {
                    powerup2.Effect(this.gameObject);
                    powerup2 = null;
                    playerHUD.RemoveItemAtIndexWithoutShifting(1);
                }
            }
        }

        public void CraftEvent(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                DeterminePowerupToBeCraftedBasedOnPlayerInput(context.ReadValue<Vector2>());
                if(powerupToBeCrafted == null)
                {
                    return;
                }
                GetRequiredChemicalsAndCraftPowerup();
            }
        }

        void CheckAllPossibleRecipes()
        {
            possibleRecipes = new();
            items = playerInventory.M_Items();
            // for each item
            for(int i = 0; i < items.Count; i++)
            {
                // for each other item
                for(int j = i + 1; j < items.Count; j++)
                {
                    // for each recipe
                    foreach(Powerup recipe in recipes)
                    {
                        // is there a recipe with two chemicals that is possible
                        if(!possibleRecipes.Contains(recipe) && TwoChemicalItemIsCraftable(i, j, recipe.recipeString))
                        {
                            possibleRecipes.Add(recipe);
                        }
                        // for each other other item
                        for(int k = j + 1; k < items.Count; k++)
                        {
                            // can you craft a recipe with three chemicals
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
            // this will be changed to be able to cycle between crafted items
            //powerupToBeCrafted = possibleRecipes.Count > 0 ? possibleRecipes[0] : null;
            
        }

        void GetRequiredChemicalsAndCraftPowerup()
        {
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
            if(powerup1 == null)
            {
                powerup1 = powerup;
                if(playerHUD)
                {
                    playerHUD.AddItemAtIndex(0, powerup.powerupSprite);
                }
            }
            else if(powerup2 == null)
            {
                powerup2 = powerup;
                playerHUD.AddItemAtIndex(1, powerup.powerupSprite);
            }
            else
            {
                return;
            }
            Debug.Log("Added powerup: " + powerup.name);
            playerInventory.RemoveItem(item1);
            playerInventory.RemoveItem(item2);
            playerInventory.RemoveItem(item3);
        }

        void DeterminePowerupToBeCraftedBasedOnPlayerInput(Vector2 keyValue)
        {
            // there is probably a better way to do this but idk right now
            // essentially a case for each key press, 1, 2, 3, and 4 and sets the powerupToBeCrafted related to that key
            if(possibleRecipes.Count > 0 && keyValue.y == 1f)
            {
                powerupToBeCrafted = possibleRecipes[0];
            }
            if(possibleRecipes.Count > 1 && keyValue.y == -1f)
            {
                powerupToBeCrafted = possibleRecipes[1];
            }
            if(possibleRecipes.Count > 2 && keyValue.x == -1f)
            {
                powerupToBeCrafted = possibleRecipes[2];
            }
            if(possibleRecipes.Count > 3 && keyValue.x == 1f)
            {
                powerupToBeCrafted = possibleRecipes[3];
            }
            if(powerupToBeCrafted)
            {
                Debug.Log(powerupToBeCrafted.name);
            }
        }

        bool PermutationIsPossible(int x, int y, int z, string recipeString) { return ChemVal(x) + ChemVal(y) + ChemVal(z) == recipeString; }

        bool TwoChemicalItemIsCraftable(int i, int j, string recipeString) { return ChemVal(i) + ChemVal(j) == recipeString || ChemVal(j) + ChemVal(i) == recipeString; }

        bool ThreeChemicalItemIsCraftable(int i, int j, int k, string recipeString) { return PermutationIsPossible(i, j, k, recipeString) || PermutationIsPossible(i, k, j, recipeString) || PermutationIsPossible(j, i, k, recipeString) || PermutationIsPossible(j, k, i, recipeString) || PermutationIsPossible(k, i, j, recipeString) || PermutationIsPossible(k, j, i, recipeString); }

        string ChemVal(int n) { return ((Chemical)items[n]).chemicalValue; }
    }
}
