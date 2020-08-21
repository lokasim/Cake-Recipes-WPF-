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
    class AddIngredientToBasketViewModel : ViewModelBase
    {
        readonly AddIngredientToRecipe addIngredientToRecipe;
        readonly AddIngredientAmountToRecipe addIngredientAmountToShoppingBasket;
        IngredientService ingrediantsData = new IngredientService();
        ShoppingService shoppingData = new ShoppingService();

        #region Constructor
        /// <summary>
        /// Opens the Add ingredient to Shopping Basket window
        /// </summary>
        /// <param name="addIngredientOpen">Window that we open</param>
        public AddIngredientToBasketViewModel(AddIngredientToRecipe addIngredientOpen)
        {
            ingredient = new tblIngredient();
            addIngredientToRecipe = addIngredientOpen;
            IngredientList = ingrediantsData.GetAllIngredients().ToList();
            IngrediantAmountList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
        }

        /// <summary>
        /// Opens the Add ingredient amount to shopping basket window
        /// </summary>
        /// <param name="addIngrediwntOpen">Window that we open</param>
        /// <param name="ingredientEdit">ingredient that we are showing</param>
        public AddIngredientToBasketViewModel(AddIngredientAmountToRecipe addIngredientAmountOpen, tblIngredient ingredientEdit)
        {
            ItemAmount = new tblShoppingBasket();
            addIngredientAmountToShoppingBasket = addIngredientAmountOpen;
            IngredientList = ingrediantsData.GetAllIngredients().ToList();
            Ingredient = ingredientEdit;
        }
        #endregion

        #region Property
        /// <summary>
        /// List of ingredients
        /// </summary>
        private List<tblIngredient> ingredientList;
        public List<tblIngredient> IngredientList
        {
            get
            {
                return ingredientList;
            }
            set
            {
                ingredientList = value;
                OnPropertyChanged("IngredientList");
            }
        }

        /// <summary>
        /// List of shopping basket
        /// </summary>
        private List<tblShoppingBasket> ingrediantAmountList;
        public List<tblShoppingBasket> IngrediantAmountList
        {
            get
            {
                return ingrediantAmountList;
            }
            set
            {
                ingrediantAmountList = value;
                OnPropertyChanged("IngrediantAmountList");
            }
        }

        /// <summary>
        /// Specific Ingredient
        /// </summary>
        private tblIngredient ingredient;
        public tblIngredient Ingredient
        {
            get
            {
                return ingredient;
            }
            set
            {
                ingredient = value;
                OnPropertyChanged("Ingredient");
            }
        }

        /// <summary>
        /// Specific Shopping Basket table row
        /// </summary>
        private tblShoppingBasket itemAmount;
        public tblShoppingBasket ItemAmount
        {
            get
            {
                return itemAmount;
            }
            set
            {
                itemAmount = value;
                OnPropertyChanged("ItemAmount");
            }
        }

        /// <summary>
        /// Amount property
        /// </summary>
        private int amount;
        public int Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Add new shopping button
        /// </summary>
        private ICommand addItem;
        public ICommand AddItem
        {
            get
            {
                if (addItem == null)
                {
                    addItem = new RelayCommand(param => AddItemExecute(), param => CanAddItemExecute());
                }
                return addItem;
            }
        }

        /// <summary>
        /// Executes the add shopping basket command
        /// </summary>
        private void AddItemExecute()
        {
            try
            {
                AddIngredientAmountToRecipe addIngredientWindow = new AddIngredientAmountToRecipe(Ingredient);
                addIngredientWindow.ShowDialog();
                // Refresh the list
                IngrediantAmountList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to add the new shopping basket
        /// </summary>
        /// <returns>true</returns>
        private bool CanAddItemExecute()
        {
            if (Ingredient.IngredientName == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Save button
        /// </summary>
        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }

        /// <summary>
        /// Saves the amount of ingredients
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                if (addIngredientAmountToShoppingBasket.txtQuantity.Text.Length < 1)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Unesite broj.", "Pogresan unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (addIngredientAmountToShoppingBasket.txtQuantity.Text.ToString() == "0" || addIngredientAmountToShoppingBasket.txtQuantity.Text.ToString() == "00")
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Kolicina mora biti veca od nule.", "Pogresan unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ItemAmount.UserID = LoggedGuest.ID;
                ItemAmount.IngredientID = Ingredient.IngredientID;
                ItemAmount.Amount = Amount;
                shoppingData.AddShoppingList(ItemAmount);
                addIngredientAmountToShoppingBasket.Close();
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce dodati sastojak...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Check if its possible to press the button
        /// </summary>
        /// <returns></returns>
        private bool CanSaveExecute()
        {
            if (addIngredientAmountToShoppingBasket.txtQuantity.Text.Trim() == "" ||
                addIngredientAmountToShoppingBasket.txtQuantity.Text.Trim() == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Close button
        /// </summary>
        private ICommand close;
        public ICommand Close
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }

        /// <summary>
        /// Exit form
        /// </summary>
        private void CloseExecute()
        {
            try
            {
                addIngredientAmountToShoppingBasket.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to close the window
        /// </summary>
        /// <returns></returns>
        private bool CanCloseExecute()
        {
            return true;
        }

        /// <summary>
        /// Delete button
        /// </summary>
        private ICommand delete;
        public ICommand Delete
        {
            get
            {
                if (delete == null)
                {
                    delete = new RelayCommand(param => DeleteExecute(), param => CanDeleteExecute());
                }
                return delete;
            }
        }

        /// <summary>
        /// Method for deleting the selected item from the list
        /// </summary>
        public void DeleteExecute()
        {
            tblIngredient ing = ingrediantsData.FindIngredient(ItemAmount.IngredientID);
            try
            {
                MessageBoxResult dialogDelete = Xceed.Wpf.Toolkit.MessageBox.Show($"Da li zelite da obrisete ovaj sastojak iz liste?\n\nSastojak: {ing.IngredientName}", "Obrisi sastojak", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (dialogDelete == MessageBoxResult.Yes)
                {
                    if (ItemAmount != null)
                    {
                        shoppingData.DeleteShoppingBasket(ItemAmount.ShoppingBasketID);
                        IngrediantAmountList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce obrisati sastojak..." + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Checks if its possible to press the delete button
        /// </summary>
        /// <returns></returns>
        public bool CanDeleteExecute()
        {
            if (ItemAmount == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Exit button
        /// </summary>
        private ICommand exit;
        public ICommand Exit
        {
            get
            {
                if (exit == null)
                {
                    exit = new RelayCommand(param => ExitExecute(), param => CanExitExecute());
                }
                return exit;
            }
        }

        /// <summary>
        /// Exits the current window
        /// </summary>
        private void ExitExecute()
        {
            MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Da li odustajete od dodavanja sastojaka\nCeo proces dodavanja u korpu ce vam biti ignorisan", "Odustani", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (dialog == MessageBoxResult.Yes)
            {               
                int counter = IngrediantAmountList.Count;
                for (int i = 0; i < counter; i++)
                {
                    shoppingData.DeleteShoppingBasket(IngrediantAmountList[i].ShoppingBasketID);
                }

                AllShoppingListViewModel.isShoppingListNotUpdated = true;
                addIngredientToRecipe.Close();
            }
        }

        /// <summary>
        /// Checks if its possible to press the button
        /// </summary>
        /// <returns></returns>
        private bool CanExitExecute()
        {
            return true;
        }

        /// <summary>
        /// AddToRecipe button
        /// </summary>
        private ICommand addToRecipe;
        public ICommand AddToRecipe
        {
            get
            {
                if (addToRecipe == null)
                {
                    addToRecipe = new RelayCommand(param => AddToRecipeExecute(), param => CanAddToRecipeExecute());
                }
                return addToRecipe;
            }
        }

        /// <summary>
        /// Checks if the users want to finish creating the recipe
        /// </summary>
        private void AddToRecipeExecute()
        {
            MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Da li zelite da sacuvate korpu?", "Zavrsetak", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (dialog == MessageBoxResult.Yes)
            {
                addIngredientToRecipe.Close();
            }
            else
            {
                shoppingData.DeleteShoppingBasket(ItemAmount.ShoppingBasketID);
            }
        }

        /// <summary>
        /// Checks if its possible to press the button
        /// </summary>
        /// <returns></returns>
        private bool CanAddToRecipeExecute()
        {
            if (IngrediantAmountList.Count == 0)
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