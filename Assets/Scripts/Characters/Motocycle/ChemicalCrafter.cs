using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
namespace ChemKart
{
    public class ChemicalCrafter : MonoBehaviour
    {
        [SerializeField] private PlayerHUD m_PlayerHUD;
        [SerializeField] private PlayerHUD m_PossibleRecipesHUD;
        [SerializeField] private Powerup[] m_Recipes;
        private List<Powerup> m_PossibleRecipes = new();
        private Powerup m_PowerupToBeCrafted;
        private Inventory m_PlayerInventory;

        private List<Collectable> m_Items;
        private Powerup m_Powerup1;
        private Powerup m_Powerup2;


        void Start()
        {
            m_PlayerInventory = transform.GetChild(0).GetComponent<Inventory>();
            m_PlayerInventory.InventoryChanged += CheckAllPossibleRecipes;
        }

        public void ItemEvent(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                float keyValue = context.ReadValue<float>();
                if(m_Powerup1 && keyValue == -1f)
                {
                    m_Powerup1.Effect(this.gameObject);
                    m_Powerup1 = null;
                    if(m_PlayerHUD)
                    {
                        m_PlayerHUD.RemoveItemAtIndexWithoutShifting(0);
                    }
                }
                if(m_Powerup2 && keyValue == 1f)
                {
                    m_Powerup2.Effect(this.gameObject);
                    m_Powerup2 = null;
                    if(m_PlayerHUD)
                    {
                        m_PlayerHUD.RemoveItemAtIndexWithoutShifting(1);
                    }
                }
            }
        }

        public void CraftEvent(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                DeterminePowerupToBeCraftedBasedOnPlayerInput(context.ReadValue<Vector2>());
                if(m_PowerupToBeCrafted == null)
                {
                    return;
                }
                GetRequiredChemicalsAndCraftPowerup();
            }
        }

        void CheckAllPossibleRecipes()
        {
            m_PossibleRecipes = new();
            m_Items = m_PlayerInventory.M_Items();
            // for each item
            for(int i = 0; i < m_Items.Count; i++)
            {
                // for each other item
                for(int j = i + 1; j < m_Items.Count; j++)
                {
                    // for each recipe
                    foreach(Powerup recipe in m_Recipes)
                    {
                        // is there a recipe with two chemicals that is possible
                        if(!m_PossibleRecipes.Contains(recipe) && TwoChemicalItemIsCraftable(i, j, recipe.recipeString))
                        {
                            m_PossibleRecipes.Add(recipe);
                        }
                        // for each other other item
                        for(int k = j + 1; k < m_Items.Count; k++)
                        {
                            // can you craft a recipe with three chemicals
                            if(!m_PossibleRecipes.Contains(recipe) && ThreeChemicalItemIsCraftable(i, j, k, recipe.recipeString))
                            {
                                m_PossibleRecipes.Add(recipe);
                            }
                        }
                    }
                }
            }
            if(!m_PossibleRecipesHUD)
            {
                return;
            }
            int index = 0;
            for(int i = 0; i < 4; i++)
            {
                m_PossibleRecipesHUD.RemoveItemAtIndex(0);
            }
            foreach(Powerup possibleRecipe in m_PossibleRecipes)
            {
                m_PossibleRecipesHUD.AddItemAtIndex(index++, possibleRecipe.powerupSprite);
            }   
        }

        void GetRequiredChemicalsAndCraftPowerup()
        {
            string recipeString = m_PowerupToBeCrafted.recipeString;
            for(int i = 0; i < m_Items.Count; i++)
            {
                for(int j = i + 1; j < m_Items.Count; j++)
                {
                    if(TwoChemicalItemIsCraftable(i, j, recipeString))
                    {
                        CraftPowerup(m_PowerupToBeCrafted, i, j, -1);
                        return;
                    }
                    for(int k = j + 1; k < m_Items.Count; k++)
                    {
                        if(ThreeChemicalItemIsCraftable(i, j, k, recipeString))
                        {
                            CraftPowerup(m_PowerupToBeCrafted, i, j, k);
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
            Chemical item1 = (Chemical)m_Items[i], item2 = (Chemical)m_Items[j], item3 = null;
            if(k >= 0)
            {
                item3 = (Chemical)m_Items[k];
            }
            if(m_Powerup1 == null)
            {
                m_Powerup1 = powerup;
                if(m_PlayerHUD)
                {
                    m_PlayerHUD.AddItemAtIndex(0, powerup.powerupSprite);
                }
            }
            else if(m_Powerup2 == null)
            {
                m_Powerup2 = powerup;
                if(m_PlayerHUD)
                {
                    m_PlayerHUD.AddItemAtIndex(1, powerup.powerupSprite);
                }
            }
            else
            {
                return;
            }
            Debug.Log("Added powerup: " + powerup.name);
            m_PlayerInventory.RemoveItem(item1);
            m_PlayerInventory.RemoveItem(item2);
            m_PlayerInventory.RemoveItem(item3);
        }

        void DeterminePowerupToBeCraftedBasedOnPlayerInput(Vector2 keyValue)
        {
            // there is probably a better way to do this but idk right now
            // essentially a case for each key press, 1, 2, 3, and 4 and sets the powerupToBeCrafted related to that key
            if(m_PossibleRecipes.Count > 0 && keyValue.y == 1f)
            {
                m_PowerupToBeCrafted = m_PossibleRecipes[0];
            }
            if(m_PossibleRecipes.Count > 1 && keyValue.y == -1f)
            {
                m_PowerupToBeCrafted = m_PossibleRecipes[1];
            }
            if(m_PossibleRecipes.Count > 2 && keyValue.x == -1f)
            {
                m_PowerupToBeCrafted = m_PossibleRecipes[2];
            }
            if(m_PossibleRecipes.Count > 3 && keyValue.x == 1f)
            {
                m_PowerupToBeCrafted = m_PossibleRecipes[3];
            }
            if(m_PowerupToBeCrafted)
            {
                Debug.Log(m_PowerupToBeCrafted.name);
            }
        }

        bool PermutationIsPossible(int x, int y, int z, string recipeString) { return ChemVal(x) + ChemVal(y) + ChemVal(z) == recipeString; }

        bool TwoChemicalItemIsCraftable(int i, int j, string recipeString) { return ChemVal(i) + ChemVal(j) == recipeString || ChemVal(j) + ChemVal(i) == recipeString; }

        bool ThreeChemicalItemIsCraftable(int i, int j, int k, string recipeString) { return PermutationIsPossible(i, j, k, recipeString) || PermutationIsPossible(i, k, j, recipeString) || PermutationIsPossible(j, i, k, recipeString) || PermutationIsPossible(j, k, i, recipeString) || PermutationIsPossible(k, i, j, recipeString) || PermutationIsPossible(k, j, i, recipeString); }

        string ChemVal(int n) { return ((Chemical)m_Items[n]).chemicalValue; }
    }
}
