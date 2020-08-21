using CakeRecipes.Models;
using CakeRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CakeRecipes.Services
{
    class ShoppingService
    {
        /// <summary>
        /// Get all data about all shopping basket ingredients from the database
        /// </summary>
        /// <returns>The list of all basket ingredients from shopping cart</returns>
        public List<tblShoppingBasket> GetAllShoppingBasketItems()
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    List<tblShoppingBasket> list = new List<tblShoppingBasket>();
                    list = (from x in context.tblShoppingBaskets select x).ToList();
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
        /// Get all data about shopping basket ingredients from selected user from the database
        /// </summary>
        /// <param name="userID">The user ID we are getting the items for</param>
        /// <returns>The list of all shopping basket ingredients from specific user</returns>
        public List<tblShoppingBasket> GetAllSelectedShoppingBasketItems(int userID)
        {
            try
            {
                List<tblShoppingBasket> list = new List<tblShoppingBasket>();
                for (int i = 0; i < GetAllShoppingBasketItems().Count; i++)
                {
                    if (GetAllShoppingBasketItems()[i].UserID == userID)
                    {
                        list.Add(GetAllShoppingBasketItems()[i]);
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
        /// Checks if the ingredient was previoulsy added
        /// </summary>
        /// <param name="ingredient">Ingredient we are checking</param>
        /// <returns>The id of the ingredient that was added or not</returns>
        public int IsIngredientAdded(tblShoppingBasket ingredient)
        {
            for (int i = 0; i < GetAllSelectedShoppingBasketItems(ingredient.UserID).Count; i++)
            {
                if (ingredient.IngredientID == GetAllSelectedShoppingBasketItems(ingredient.UserID)[i].IngredientID &&
                        ingredient.UserID == GetAllSelectedShoppingBasketItems(ingredient.UserID)[i].UserID)
                {
                    return GetAllSelectedShoppingBasketItems(ingredient.UserID)[i].ShoppingBasketID;
                }
            }
            return 0;
        }

        /// <summary>
        /// Adds ingredients to shopping list to the database
        /// </summary>
        /// <param name="shoppingBasket">The shopping basket we are adding or editing</param>
        /// <returns>The new or edited shopping basket</returns>
        public tblShoppingBasket AddShoppingList(tblShoppingBasket shoppingBasket)
        {
            // Delete ingredient if it was added earlier
            if (IsIngredientAdded(shoppingBasket) != 0)
            {
                DeleteShoppingBasket(IsIngredientAdded(shoppingBasket));
            }

            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    if (shoppingBasket.ShoppingBasketID == 0)
                    {
                        tblShoppingBasket newShoppingBasket = new tblShoppingBasket
                        {
                            UserID = LoggedGuest.ID,
                            IngredientID = shoppingBasket.IngredientID,
                            Amount = shoppingBasket.Amount
                        };

                        context.tblShoppingBaskets.Add(newShoppingBasket);
                        context.SaveChanges();
                        shoppingBasket.ShoppingBasketID = newShoppingBasket.ShoppingBasketID;

                        return shoppingBasket;
                    }
                    else
                    {
                        tblShoppingBasket shoppingBasketEdit = (from ss in context.tblShoppingBaskets where ss.ShoppingBasketID == shoppingBasket.ShoppingBasketID select ss).First();
                        shoppingBasketEdit.UserID = LoggedGuest.ID;
                        shoppingBasketEdit.IngredientID = shoppingBasket.IngredientID;
                        shoppingBasketEdit.Amount = shoppingBasket.Amount;

                        context.SaveChanges();

                        return shoppingBasket;
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
        /// Deletes shopping basket
        /// </summary>
        /// <param name="shoppingBasketID">the ingredientAmount that is being deleted</param>
        public void DeleteShoppingBasket(int shoppingBasketID)
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    for (int i = 0; i < GetAllShoppingBasketItems().Count; i++)
                    {
                        if (GetAllShoppingBasketItems().ToList()[i].ShoppingBasketID == shoppingBasketID)
                        {
                            tblShoppingBasket shoppingBasketToDelete = (from ss in context.tblShoppingBaskets where ss.ShoppingBasketID == shoppingBasketID select ss).First();
                            context.tblShoppingBaskets.Remove(shoppingBasketToDelete);
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
