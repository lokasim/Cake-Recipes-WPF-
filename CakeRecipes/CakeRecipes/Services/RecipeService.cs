using CakeRecipes.Models;
using CakeRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CakeRecipes.Services
{
    /// <summary>
    /// Class used to create the CRUD structure for Recipes
    /// </summary>
    class RecipeService
    {
        /// <summary>
        /// Get all data about recipes from the database
        /// </summary>
        /// <returns>The list of all recipes</returns>
        public List<tblRecipe> GetAllRecipes()
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    List<tblRecipe> list = new List<tblRecipe>();
                    list = (from x in context.tblRecipes select x).ToList();
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
        /// Get all data about recipes from the current user
        /// </summary>
        /// <returns>The list of all recipes</returns>
        public List<tblRecipe> GetAllRecipesFromCurrentUser(int userID)
        {
            List<tblRecipe> list = new List<tblRecipe>();
            for (int i = 0; i < GetAllRecipes().Count; i++)
            {
                if (GetAllRecipes()[i].UserID == userID)
                {
                    list.Add(GetAllRecipes()[i]);
                }
            }
            return list;
        }

        /// <summary>
        /// Get all data about all ingredients in recipes from the database
        /// </summary>
        /// <returns>The list of all ingredients from recipes</returns>
        public List<tblIngredientAmount> GetAllRecipeIngrediant()
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    List<tblIngredientAmount> list = new List<tblIngredientAmount>();
                    list = (from x in context.tblIngredientAmounts select x).ToList();
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
        /// Get all data about ingredients from selected recipe from the database
        /// </summary>
        /// <param name="recipe">The recipe ID we are getting the ingredients from</param>
        /// <returns>The list of all ingredients from recipes</returns>
        public List<tblIngredientAmount> GetAllSelectedRecipeIngrediantAmount(int recipeID)
        {
            try
            {
                List<tblIngredientAmount> list = new List<tblIngredientAmount>();
                for (int i = 0; i < GetAllRecipeIngrediant().Count; i++)
                {
                    if (GetAllRecipeIngrediant()[i].RecipeID == recipeID)
                    {
                        list.Add(GetAllRecipeIngrediant()[i]);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Adds an recipes to the database
        /// </summary>
        /// <param name="recipe">The recipe ID we are adding or editing</param>
        /// <returns>The new or edited recipe</returns>
        public tblRecipe AddRecipe(tblRecipe recipe)
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    if (recipe.RecipeID == 0)
                    {
                        tblRecipe newRecipe = new tblRecipe();
                        newRecipe.RecipeName = recipe.RecipeName;
                        newRecipe.RecipeType = recipe.RecipeType;
                        newRecipe.NoPeople = recipe.NoPeople;
                        newRecipe.RecipeDescription = recipe.RecipeDescription;

                        if (recipe.CreationDate != default(DateTime))
                        {
                            newRecipe.CreationDate = recipe.CreationDate;
                        }
                        else
                        {
                            newRecipe.CreationDate = DateTime.Now;
                        }

                        if (recipe.Changed != null)
                        {
                            newRecipe.Changed = recipe.Changed;
                        }
                        else
                        {
                            newRecipe.Changed = LoggedGuest.NameSurname;
                        }

                        if (recipe.UserID != null)
                        {
                            newRecipe.UserID = recipe.UserID;
                        }
                        else
                        {
                            newRecipe.UserID = LoggedGuest.ID;
                        }
                            
                        context.tblRecipes.Add(newRecipe);
                        context.SaveChanges();
                        recipe.RecipeID = newRecipe.RecipeID;

                        return recipe;
                    }
                    else
                    {
                        tblRecipe recipeToEdit = (from ss in context.tblRecipes where ss.RecipeID == recipe.RecipeID select ss).First();
                        recipeToEdit.RecipeName = recipe.RecipeName;
                        recipeToEdit.RecipeType = recipe.RecipeType;
                        recipeToEdit.NoPeople = recipe.NoPeople;
                        recipeToEdit.RecipeDescription = recipe.RecipeDescription;
                        recipeToEdit.CreationDate = DateTime.Now;
                        recipeToEdit.Changed = LoggedGuest.NameSurname;

                        if (LoggedGuest.ID == 0)
                        {
                            recipeToEdit.UserID = recipe.UserID;                          
                        }
                        else
                        {
                            recipeToEdit.UserID = LoggedGuest.ID;
                        }
                                              
                        context.SaveChanges();

                        return recipe;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Deletes recipe
        /// </summary>
        /// <param name="recipeID">the recipe that is being deleted</param>
        public void DeleteRecipe(int recipeID)
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    for (int i = 0; i < GetAllRecipes().Count; i++)
                    {
                        if (GetAllRecipes().ToList()[i].RecipeID == recipeID)
                        {
                            // Remove all recipe ingredients before the recipe
                            int selectedRecipeIngrediantAmountCount = GetAllSelectedRecipeIngrediantAmount(recipeID).Count;

                            for (int j = 0; j < selectedRecipeIngrediantAmountCount; j++)
                            {
                                tblIngredientAmount ingAmountToDelete = (from ss in context.tblIngredientAmounts where ss.RecipeID == recipeID select ss).First();
                                context.tblIngredientAmounts.Remove(ingAmountToDelete);
                                context.SaveChanges();
                            }

                            tblRecipe recipeToDelete = (from r in context.tblRecipes where r.RecipeID == recipeID select r).First();
                            context.tblRecipes.Remove(recipeToDelete);
                            context.SaveChanges();

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// Checks if the ingredient was previoulsy added
        /// </summary>
        /// <param name="ingredientAmount">Ingredient we are checking</param>
        /// <returns>The id of the ingredient that was added or not</returns>
        public int IsIngredientAdded(tblIngredientAmount ingredientAmount)
        {
            for (int i = 0; i < GetAllSelectedRecipeIngrediantAmount(ingredientAmount.RecipeID).Count; i++)
            {
                if (ingredientAmount.IngredientID == GetAllSelectedRecipeIngrediantAmount(ingredientAmount.RecipeID)[i].IngredientID &&
                        ingredientAmount.RecipeID == GetAllSelectedRecipeIngrediantAmount(ingredientAmount.RecipeID)[i].RecipeID)
                {
                    return GetAllSelectedRecipeIngrediantAmount(ingredientAmount.RecipeID)[i].IngredientAmountID;
                }
            }
            return 0;
        }

        /// <summary>
        /// Adds ingredients to recipes to the database
        /// </summary>
        /// <param name="ingredientAmount">The ingredient Amount ID we are adding or editing</param>
        /// <returns>The new or edited ingredient amount</returns>
        public tblIngredientAmount AddIngredientAmount(tblIngredientAmount ingredientAmount)
        {
            // Delete ingredient if it was added earlier
            if (IsIngredientAdded(ingredientAmount) != 0)
            {
                DeleteIngredientAmount(IsIngredientAdded(ingredientAmount));
            }

            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    if (ingredientAmount.IngredientAmountID == 0)
                    {
                        tblIngredientAmount newIngredientAmount = new tblIngredientAmount
                        {
                            RecipeID = ingredientAmount.RecipeID,
                            IngredientID = ingredientAmount.IngredientID,
                            Amount = ingredientAmount.Amount
                        };

                        context.tblIngredientAmounts.Add(newIngredientAmount);
                        context.SaveChanges();
                        ingredientAmount.IngredientAmountID = newIngredientAmount.IngredientAmountID;

                        return ingredientAmount;
                    }
                    else
                    {
                        tblIngredientAmount ingredientAmountEdit = (from ss in context.tblIngredientAmounts where ss.IngredientAmountID == ingredientAmount.IngredientAmountID select ss).First();
                        ingredientAmountEdit.RecipeID = ingredientAmount.RecipeID;
                        ingredientAmountEdit.IngredientID = ingredientAmount.IngredientID;
                        ingredientAmountEdit.Amount = ingredientAmount.Amount;

                        context.SaveChanges();

                        return ingredientAmount;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Deletes recipe
        /// </summary>
        /// <param name="ingredientAmountID">the ingredientAmount that is being deleted</param>
        public void DeleteIngredientAmount(int ingredientAmountID)
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    for (int i = 0; i < GetAllRecipeIngrediant().Count; i++)
                    {
                        if (GetAllRecipeIngrediant().ToList()[i].IngredientAmountID == ingredientAmountID)
                        {
                            tblIngredientAmount ingAmountToDelete = (from ss in context.tblIngredientAmounts where ss.IngredientAmountID == ingredientAmountID select ss).First();
                            context.tblIngredientAmounts.Remove(ingAmountToDelete);
                            context.SaveChanges();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }
    }
}
