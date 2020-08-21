using CakeRecipes.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CakeRecipes.Services
{
    /// <summary>
    /// Class used to create the CRUD structure for Ingredients
    /// </summary>
    class IngredientService
    {
        /// <summary>
        /// Get all data about ingredients from the database
        /// </summary>
        /// <returns>The list of all ingredients</returns>
        public List<tblIngredient> GetAllIngredients()
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    List<tblIngredient> list = new List<tblIngredient>();
                    list = (from x in context.tblIngredients select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Adds an ingredient to the database
        /// </summary>
        /// <param name="ingredient">The ingredient ID we are adding or editing</param>
        /// <returns>The new or edited ingredient</returns>
        public tblIngredient AddIngredient(tblIngredient ingredient)
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    tblIngredient newIngredient = new tblIngredient
                    {
                        IngredientName = ingredient.IngredientName
                    };

                    context.tblIngredients.Add(newIngredient);
                    context.SaveChanges();
                    ingredient.IngredientID = newIngredient.IngredientID;

                    return ingredient;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Search if ingredient with that ID exists in the ingredient table
        /// </summary>
        /// <param name="id">Takes the id that we want to search for</param>
        /// <returns>The ingredient</returns>
        public tblIngredient FindIngredient(int id)
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    tblIngredient result = (from x in context.tblIngredients where x.IngredientID == id select x).FirstOrDefault();

                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception " + ex.Message.ToString());
                return null;
            }
        }
    }
}
