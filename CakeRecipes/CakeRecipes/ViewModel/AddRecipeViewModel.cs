using CakeRecipes.Command;
using CakeRecipes.Models;
using CakeRecipes.Services;
using CakeRecipes.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CakeRecipes.ViewModel
{
    class AddRecipeViewModel : ViewModelBase
    {
        readonly AddRecipe addRecipe;
        RecipeData recipesData = new RecipeData();
        IngredientsData ingrediantsData = new IngredientsData();

        #region Commands
        /// <summary>
        /// Opens the Add recipe window
        /// </summary>
        /// <param name="addRecipeOpen">Window that we open</param>
        public AddRecipeViewModel(AddRecipe addRecipeOpen)
        {
            recipe = new tblRecipe();
            addRecipe = addRecipeOpen;
            IngredientList = ingrediantsData.GetAllIngredients().ToList();
        }
        #endregion

        #region Property
        /// <summary>
        /// Specific recipe
        /// </summary>
        private tblRecipe recipe;
        public tblRecipe Recipe
        {
            get
            {
                return recipe;
            }
            set
            {
                recipe = value;
                OnPropertyChanged("Recipe");
            }
        }

        private bool isUpdateRecipe;
        public bool IsUpdateRecipe
        {
            get
            {
                return isUpdateRecipe;
            }
            set
            {
                isUpdateRecipe = value;
            }
        }

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
        /// Method for adding new Recipe
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                recipesData.AddRecipe(Recipe);
                isUpdateRecipe = true;

                AddIngredientToRecipe addIngredientWindow = new AddIngredientToRecipe(Recipe.RecipeID);
                addRecipe.Close();
                addIngredientWindow.ShowDialog();
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
            if (String.IsNullOrEmpty(Recipe.RecipeName) ||
                String.IsNullOrEmpty(Recipe.RecipeType) ||
                String.IsNullOrEmpty(Recipe.RecipeDescription))
            {
                return false;
            }
            else if (Convert.ToInt32(Recipe.NoPeople) < 1)
            {
                addRecipe.error.Text = "Broj osoba ne moze biti manji od 1";
                addRecipe.error.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                addRecipe.error.Visibility = Visibility.Collapsed;
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
                addRecipe.Close();
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
            MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Da li odustajete od kreiranja recepta?", "Odustani", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (dialog == MessageBoxResult.Yes)
            {
                addRecipe.Close();
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
