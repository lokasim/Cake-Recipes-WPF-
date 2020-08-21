using CakeRecipes.Command;
using CakeRecipes.Helper;
using CakeRecipes.Models;
using CakeRecipes.Services;
using CakeRecipes.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace CakeRecipes.ViewModel
{
    class AddIngredientViewModel : ViewModelBase
    {
        readonly AddIngredient addIngredient;
        IngredientService ingrediantsData = new IngredientService();

        #region Constructor
        /// <summary>
        /// Opens the Add ingredient window
        /// </summary>
        /// <param name="addIngredientOpen">Window that we open</param>
        /// <param name="recipeID">recipe id</param>
        public AddIngredientViewModel(AddIngredient addIngredientOpen)
        {
            ingredient = new tblIngredient();
            addIngredient = addIngredientOpen;
        }
        #endregion

        #region Property
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

        private bool isUpdateIngredient;
        public bool IsUpdateIngredient
        {
            get
            {
                return isUpdateIngredient;
            }
            set
            {
                isUpdateIngredient = value;
            }
        }  
        #endregion

        #region Commands
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
        /// Method for adding new Ingredients
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                Validations val = new Validations();
                if (val.IngredientNameChecker(Ingredient.IngredientName) != null)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(val.IngredientNameChecker(Ingredient.IngredientName), "Naziv sastojka");
                    return;
                }

                ingrediantsData.AddIngredient(Ingredient);
                isUpdateIngredient = true;
                AddRecipe test = new AddRecipe();
                addIngredient.Close();

                test.Show();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if it is possible to click on the button
        /// </summary>
        /// <returns></returns>
        private bool CanSaveExecute()
        {
            if (String.IsNullOrEmpty(ingredient.IngredientName))
            {
                return false;
            }
            else
            {
                addIngredient.error.Visibility = Visibility.Collapsed;
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
        /// Exit window
        /// </summary>
        private void CloseExecute()
        {
            try
            {
                addIngredient.Close();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if it is possible to click on the button
        /// </summary>
        /// <returns></returns>
        private bool CanCloseExecute()
        {
            return true;
        }
        #endregion
    }
}
