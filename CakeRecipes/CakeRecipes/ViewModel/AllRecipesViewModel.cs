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
    class AllRecipesViewModel : ViewModelBase
    {
        RecipeService recipeData = new RecipeService();
        IngredientService ingredientData = new IngredientService();
        readonly AllRecipesWindow allReciperWindow;
        public static bool isRecipeNotUpdated = false;

        #region Constructor
        /// <summary>
        /// Constructor with the AllRecipesWindow info window opening
        /// </summary>
        /// <param name="allRecipesWindowOpen">opends the window</param>
        public AllRecipesViewModel(AllRecipesWindow allRecipesWindowOpen)
        {
            allReciperWindow = allRecipesWindowOpen;
            RecipeList = recipeData.GetAllRecipes().ToList();
        }
        #endregion

        #region Property
        /// <summary>
        /// List of recipes
        /// </summary>
        private List<tblRecipe> recipeList;
        public List<tblRecipe> RecipeList
        {
            get
            {
                return recipeList;
            }
            set
            {
                recipeList = value;
                OnPropertyChanged("RecipeList");
            }
        }

        /// <summary>
        /// Specific Recipe
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
        #endregion

        #region Commands
        /// <summary>
        /// Delete Recipe button
        /// </summary>
        private ICommand deleteRecipe;
        public ICommand DeleteRecipe
        {
            get
            {
                if (deleteRecipe == null)
                {
                    deleteRecipe = new RelayCommand(param => DeleteRecipeExecute(), param => CanDeleteRecipeExecute());
                }
                return deleteRecipe;
            }
        }

        /// <summary>
        /// Method for deleting the selected item from the list
        /// </summary>
        public void DeleteRecipeExecute()
        {
            try
            {
                MessageBoxResult dialogDelete = Xceed.Wpf.Toolkit.MessageBox.Show($"Da li zelite da obrisete ovaj recept iz liste?\n\nRecept: {Recipe.RecipeName}", "Obrisi recept", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (dialogDelete == MessageBoxResult.Yes)
                {
                    if (Recipe != null)
                    {
                        recipeData.DeleteRecipe(Recipe.RecipeID);
                        RecipeList = recipeData.GetAllRecipes().ToList();
                    }
                }
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce obrisati recept...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if its possible to press the delete button
        /// </summary>
        /// <returns></returns>
        public bool CanDeleteRecipeExecute()
        {
            if (Recipe == null || LoggedGuest.NameSurname != "Administrator")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Edit Recipe button
        /// </summary>
        private ICommand editRecipe;
        public ICommand EditRecipe
        {
            get
            {
                if (editRecipe == null)
                {
                    editRecipe = new RelayCommand(param => EditRecipeExecute(), param => CanEditRecipeExecute());
                }
                return editRecipe;
            }
        }

        /// <summary>
        /// Method for edit the selected item from the list
        /// </summary>
        public void EditRecipeExecute()
        {
            tblRecipe tempRecipe = new tblRecipe
            {
                RecipeID = 0,
                RecipeName = Recipe.RecipeName,
                RecipeType = Recipe.RecipeType,
                NoPeople = Recipe.NoPeople,
                RecipeDescription = Recipe.RecipeDescription,
                CreationDate = Recipe.CreationDate,
                UserID = Recipe.UserID,
                Changed = Recipe.Changed
            };
            
            List<tblIngredientAmount> tempRecipeIngrediantAmountList = recipeData.GetAllSelectedRecipeIngrediantAmount(recipe.RecipeID).ToList();

            try
            {
                MessageBoxResult dialogDelete = Xceed.Wpf.Toolkit.MessageBox.Show($"Da li zelite da azurirate ovaj recept iz liste?\n\nRecept: {Recipe.RecipeName}", "Azuriraj recept", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (dialogDelete == MessageBoxResult.Yes)
                {
                    if (Recipe != null)
                    {
                        AddRecipe addRecipeWindow = new AddRecipe(Recipe);
                        addRecipeWindow.ShowDialog();

                        // Checks if the recipe did not get updated
                        if(isRecipeNotUpdated == true)
                        {
                            recipeData.AddRecipe(tempRecipe);
                            for (int i = 0; i < tempRecipeIngrediantAmountList.Count; i++)
                            {
                                tempRecipeIngrediantAmountList[i].IngredientAmountID = 0;
                                tempRecipeIngrediantAmountList[i].RecipeID = tempRecipe.RecipeID;
                                recipeData.AddIngredientAmount(tempRecipeIngrediantAmountList[i]);
                            }
                            isRecipeNotUpdated = false;
                        }
                        RecipeList = recipeData.GetAllRecipes().ToList();
                    }
                }
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce obrisati recept...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if its possible to press the edit button
        /// </summary>
        /// <returns></returns>
        public bool CanEditRecipeExecute()
        {
            if (Recipe == null)
            {
                return false;
            }
            else if (LoggedGuest.ID == Recipe.UserID || LoggedGuest.NameSurname == "Administrator")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Add Recipe button
        /// </summary>
        private ICommand addRecipe;
        public ICommand AddRecipe
        {
            get
            {
                if (addRecipe == null)
                {
                    addRecipe = new RelayCommand(param => AddRecipeExecute(), param => CanAddRecipeExecute());
                }
                return addRecipe;
            }
        }

        /// <summary>
        /// Method for adding the selected item from the list
        /// </summary>
        public void AddRecipeExecute()
        {
            try
            {
                AddRecipe addRecipeWindow = new AddRecipe();
                addRecipeWindow.ShowDialog();
                RecipeList = recipeData.GetAllRecipes().ToList();
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce dodati recept...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if its possible to press the add button
        /// </summary>
        /// <returns></returns>
        public bool CanAddRecipeExecute()
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
