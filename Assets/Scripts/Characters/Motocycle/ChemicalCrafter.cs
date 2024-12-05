using UnityEngine;
using System.Collections.Generic;
namespace ChemKart
{
    public class ChemicalCrafter : MonoBehaviour
    {
        public Powerup[] recipes;
        List<string> possibleRecipes = new();
        Inventory inventory;

        List<Collectable> items;

        void Start()
        {
            inventory = transform.GetChild(0).GetComponent<Inventory>();
            inventory.InventoryChanged += CheckAllPossibleRecipes;
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
                        if(!possibleRecipes.Contains(recipe.recipeString) && TwoChemicalItemIsCraftable(i, j, recipe.recipeString))
                        {
                            possibleRecipes.Add(recipe.recipeString);
                        }
                    }
                    for(int k = j + 1; k < items.Count; k++)
                    {
                        foreach(Powerup recipe in recipes)
                        {
                            if(!possibleRecipes.Contains(recipe.recipeString) && ThreeChemicalItemIsCraftable(i, j, k, recipe.recipeString))
                            {
                                possibleRecipes.Add(recipe.recipeString);
                            }
                        }
                    }
                }
            }
            foreach(string possibleRecipe in possibleRecipes)
            {
                Debug.Log("Possible recipe: " + possibleRecipe);
            }
        }

        bool PermutationIsPossible(int x, int y, int z, string recipeString) { return ChemVal(x) + ChemVal(y) + ChemVal(z) == recipeString; }

        bool TwoChemicalItemIsCraftable(int i, int j, string recipeString) { return ChemVal(i) + ChemVal(j) == recipeString || ChemVal(j) + ChemVal(i) == recipeString; }

        bool ThreeChemicalItemIsCraftable(int i, int j, int k, string recipeString) { return PermutationIsPossible(i, j, k, recipeString) || PermutationIsPossible(i, k, j, recipeString) || PermutationIsPossible(j, i, k, recipeString) || PermutationIsPossible(j, k, i, recipeString) || PermutationIsPossible(k, i, j, recipeString) || PermutationIsPossible(k, j, i, recipeString); }

        string ChemVal(int n) { return ((Chemical)items[n]).chemicalValue; }
    }
}
