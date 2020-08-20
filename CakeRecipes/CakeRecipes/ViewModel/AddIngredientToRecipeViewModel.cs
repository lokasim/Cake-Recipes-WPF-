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
    class AddIngredientToRecipeViewModel : ViewModelBase
    {
        readonly AddIngredientToRecipe addIngredientToRecipe;
        IngredientsData ingrediantsData = new IngredientsData();

        #region Constructor
        /// <summary>
        /// Opens the Add ingredient to recipe window
        /// </summary>
        /// <param name="addIngrediwntOpen">Window that we open</param>
        public AddIngredientToRecipeViewModel(AddIngredientToRecipe addIngredientOpen)
        {
            ingredient = new tblIngredient();
            addIngredientToRecipe = addIngredientOpen;
            IngredientList = ingrediantsData.GetAllIngredients().ToList();
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
        #endregion

        #region Commands
        private ICommand addItem;
        public ICommand AddItem
        {
            get
            {
                if (addItem == null)
                {
                    //addItem = new RelayCommand(param => AddItemExecute(), param => CanAddItemExecute());
                }
                return addItem;
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
            MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Da li odustajete od dodavanja sastojaka?", "Odustani", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (dialog == MessageBoxResult.Yes)
            {
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
        #endregion
    }
}
