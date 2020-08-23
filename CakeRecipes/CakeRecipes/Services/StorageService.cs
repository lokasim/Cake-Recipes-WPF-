using CakeRecipes.Models;
using CakeRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CakeRecipes.Services
{
    class StorageService
    {

        #region Delegate and Event
        // publisher
        public delegate void Notification(tblShoppingBasket item);
        public event Notification OnNotification;

        //raises an event
        internal void Notify(tblShoppingBasket item)
        {
            OnNotification?.Invoke(item);
        }
        #endregion


        /// <summary>
        /// Get all data about all ingredient storage ingredients from the database
        /// </summary>
        /// <returns>The list of all ingredient storage ingredients from storage</returns>
        public List<tblIngredientStorage> GetAllIngredientStorageItems()
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    List<tblIngredientStorage> list = new List<tblIngredientStorage>();
                    list = (from x in context.tblIngredientStorages select x).ToList();
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
        /// Get all data about ingredient storage ingredients from selected user from the database
        /// </summary>
        /// <param name="userID">The user ID we are getting the items for</param>
        /// <returns>The list of all ingredient storage ingredients from specific user</returns>
        public List<tblIngredientStorage> GetAllSelectedIngredientStorageItems(int userID)
        {
            try
            {
                List<tblIngredientStorage> list = new List<tblIngredientStorage>();
                for (int i = 0; i < GetAllIngredientStorageItems().Count; i++)
                {
                    // admin
                    if (LoggedGuest.ID == 0 && GetAllIngredientStorageItems()[i].UserID == null)
                    {
                        GetAllIngredientStorageItems()[i].UserID = 0;
                        list.Add(GetAllIngredientStorageItems()[i]);
                    }

                    else if (GetAllIngredientStorageItems()[i].UserID == userID)
                    {
                        list.Add(GetAllIngredientStorageItems()[i]);
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

        public void AddIngredientStorage(tblShoppingBasket item)
        {
            try
            {
                using (CakeRecipesDBEntities context = new CakeRecipesDBEntities())
                {
                    if (IngredientExistInStorage(item.IngredientID, LoggedGuest.ID) == false)
                    {
                        tblIngredientStorage newStorage = new tblIngredientStorage
                        {
                            UserID = LoggedGuest.ID,
                            IngredientID = item.IngredientID,
                            Amount = item.Amount
                        };

                        if (LoggedGuest.ID == 0)
                        {
                            newStorage.UserID = null;
                        }

                        context.tblIngredientStorages.Add(newStorage);
                        context.SaveChanges();
                    }
                    else
                    {
                        tblIngredientStorage storageEdit = (from ss in context.tblIngredientStorages where ss.IngredientID == item.IngredientID where ss.UserID == item.UserID select ss).First();

                        if (LoggedGuest.ID == 0)
                        {
                            storageEdit.UserID = item.UserID;
                        }
                        else
                        {
                            storageEdit.UserID = LoggedGuest.ID;
                        }

                        storageEdit.IngredientID = item.IngredientID;
                        storageEdit.Amount = item.Amount + storageEdit.Amount;

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        public bool IngredientExistInStorage(int item, int id)
        {
            for (int i = 0; i < GetAllSelectedIngredientStorageItems(id).Count; i++)
            {
                if (GetAllSelectedIngredientStorageItems(id)[i].IngredientID == item)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
