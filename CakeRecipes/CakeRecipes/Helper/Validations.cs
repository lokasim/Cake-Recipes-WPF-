using CakeRecipes.Models;
using CakeRecipes.Services;
using System.Collections.Generic;

namespace CakeRecipes.Helper
{
    /// <summary>
    /// Validates the user inputs
    /// </summary>
    class Validations
    {
        /// <summary>
        /// Checks if the Ingredient Name exists
        /// </summary>
        /// <param name="ingredientName">the ingredient name we are checking</param>
        /// <returns>null if the input is correct or string error message if its wrong</returns>
        public string IngredientNameChecker(string ingredientName)
        {
            IngredientService ingredientData = new IngredientService();
            List<tblIngredient> AllIngredients = ingredientData.GetAllIngredients();

            if (ingredientName == null)
            {
                return "Naziv sastojka ne moze biti prazno.";
            }

            // Check if the ingredient name already exists
            for (int i = 0; i < AllIngredients.Count; i++)
            {
                if (AllIngredients[i].IngredientName.ToLower() == ingredientName.ToLower())
                {
                    return "Naziv sastojka vec postoji.";
                }
            }

            return null;
        }
    }
}
