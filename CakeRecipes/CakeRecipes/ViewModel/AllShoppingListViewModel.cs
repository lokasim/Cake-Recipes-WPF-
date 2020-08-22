using CakeRecipes.Command;
using CakeRecipes.Models;
using CakeRecipes.Services;
using CakeRecipes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CakeRecipes.ViewModel
{
    class AllShoppingListViewModel : ViewModelBase
    {
        ShoppingService shoppingData = new ShoppingService();
        IngredientService ingredientData = new IngredientService();
        readonly AllShoppingList allShoppingListWindow;
        public static bool isShoppingListNotUpdated = false;

        #region Constructor
        /// <summary>
        /// Constructor with the AllShoppingList info window opening
        /// </summary>
        /// <param name="allShoppingWindowOpen">opends the window</param>
        public AllShoppingListViewModel(AllShoppingList allShoppingWindowOpen)
        {
            allShoppingListWindow = allShoppingWindowOpen;
            ShoppingBasketList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
        }
        #endregion

        #region Property
        /// <summary>
        /// List of recipes
        /// </summary>
        private List<tblShoppingBasket> shoppingBasketList;
        public List<tblShoppingBasket> ShoppingBasketList
        {
            get
            {
                return shoppingBasketList;
            }
            set
            {
                shoppingBasketList = value;
                OnPropertyChanged("ShoppingBasketList");
            }
        }

        /// <summary>
        /// Specific shopping Basket
        /// </summary>
        private tblShoppingBasket shoppingBasket;
        public tblShoppingBasket ShoppingBasket
        {
            get
            {
                return shoppingBasket;
            }
            set
            {
                shoppingBasket = value;
                OnPropertyChanged("tblShoppingBasket");
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Delete Shopping Basket button
        /// </summary>
        private ICommand deleteShoppingBasket;
        public ICommand DeleteShoppingBasket
        {
            get
            {
                if (deleteShoppingBasket == null)
                {
                    deleteShoppingBasket = new RelayCommand(param => DeleteShoppingBasketExecute(), param => CanDeleteShoppingBasketExecute());
                }
                return deleteShoppingBasket;
            }
        }

        /// <summary>
        /// Method for deleting the selected item from the list
        /// </summary>
        public void DeleteShoppingBasketExecute()
        {
            try
            {
                MessageBoxResult dialogDelete = Xceed.Wpf.Toolkit.MessageBox.Show($"Da li zelite da obrisete ovaj sastojak iz liste?", "Obrisi recept", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (dialogDelete == MessageBoxResult.Yes)
                {
                    if (ShoppingBasket != null)
                    {
                        shoppingData.DeleteShoppingBasket(ShoppingBasket.ShoppingBasketID);
                        ShoppingBasketList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
                    }
                }
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce obrisati sastojak...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if its possible to press the delete button
        /// </summary>
        /// <returns></returns>
        public bool CanDeleteShoppingBasketExecute()
        {
            if (ShoppingBasket == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Add Shopping Item button
        /// </summary>
        private ICommand addShoppingItem;
        public ICommand AddShoppingItem
        {
            get
            {
                if (addShoppingItem == null)
                {
                    addShoppingItem = new RelayCommand(param => AddShoppingItemExecute(), param => CanAddShoppingItemExecute());
                }
                return addShoppingItem;
            }
        }

        /// <summary>
        /// Method for adding the selected item from the list
        /// </summary>
        public void AddShoppingItemExecute()
        {
            try
            {
                List<tblShoppingBasket> tempShoppingList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
                AddIngredientToRecipeWindow addIngredientToRecipeWindow = new AddIngredientToRecipeWindow();
                addIngredientToRecipeWindow.ShowDialog();

                // Return the list to the initial state
                if (isShoppingListNotUpdated == true)
                {
                    for (int i = 0; i < tempShoppingList.Count; i++)
                    {
                        tempShoppingList[i].ShoppingBasketID = 0;
                        tempShoppingList[i].UserID = LoggedGuest.ID;
                        shoppingData.AddShoppingList(tempShoppingList[i]);
                    }
                    isShoppingListNotUpdated = false;
                }

                ShoppingBasketList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce dodati sastojak...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if its possible to press the add button
        /// </summary>
        /// <returns></returns>
        public bool CanAddShoppingItemExecute()
        {
            if (ingredientData.GetAllIngredients().Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
