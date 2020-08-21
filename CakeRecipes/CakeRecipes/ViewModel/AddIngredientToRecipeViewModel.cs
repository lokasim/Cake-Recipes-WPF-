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
        readonly AddIngredientAmountToRecipe addIngredientAmountToRecipe;
        IngredientService ingrediantsData = new IngredientService();
        RecipeService recipeData = new RecipeService();

        #region Constructor
        /// <summary>
        /// Opens the Add ingredient to recipe window
        /// </summary>
        /// <param name="addIngrediwntOpen">Window that we open</param>
        public AddIngredientToRecipeViewModel(AddIngredientToRecipe addIngredientOpen, int recipeIDEdit)
        {
            ingredient = new tblIngredient();
            addIngredientToRecipe = addIngredientOpen;
            IngredientList = ingrediantsData.GetAllIngredients().ToList();
            RecipeIngrediantAmountList = recipeData.GetAllSelectedRecipeIngrediantAmount(recipeIDEdit).ToList();
            RecipeID = recipeIDEdit;
        }

        /// <summary>
        /// Opens the Add ingredient amount to recipe window
        /// </summary>
        /// <param name="addIngrediwntOpen">Window that we open</param>
        /// <param name="ingredientEdit">ingredient that we are showing</param>
        /// <param name="recipeIDEdit">recipe that we are showing</param>
        public AddIngredientToRecipeViewModel(AddIngredientAmountToRecipe addIngredientAmountOpen, tblIngredient ingredientEdit, int recipeIDEdit)
        {
            ingredientAmount = new tblIngredientAmount();
            addIngredientAmountToRecipe = addIngredientAmountOpen;
            IngredientList = ingrediantsData.GetAllIngredients().ToList();
            Ingredient = ingredientEdit;
            RecipeID = recipeIDEdit;
        }
        #endregion

        #region Property
        /// <summary>
        /// Specific Recipe
        /// </summary>
        private int recipeID;
        public int RecipeID
        {
            get
            {
                return recipeID;
            }
            set
            {
                recipeID = value;
                OnPropertyChanged("RecipeID");
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
        /// List of ingredients
        /// </summary>
        private List<tblIngredientAmount> recipeIngrediantAmountList;
        public List<tblIngredientAmount> RecipeIngrediantAmountList
        {
            get
            {
                return recipeIngrediantAmountList;
            }
            set
            {
                recipeIngrediantAmountList = value;
                OnPropertyChanged("RecipeIngrediantAmountList");
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
        /// Specific Ingredient Amount table row
        /// </summary>
        private tblIngredientAmount ingredientAmount;
        public tblIngredientAmount IngredientAmount
        {
            get
            {
                return ingredientAmount;
            }
            set
            {
                ingredientAmount = value;
                OnPropertyChanged("IngredientAmount");
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Add new ingredient button
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
        /// Executes the add ingredient command
        /// </summary>
        private void AddItemExecute()
        {
            try
            {
                AddIngredientAmountToRecipe addIngredientWindow = new AddIngredientAmountToRecipe(Ingredient, recipeID);
                addIngredientWindow.ShowDialog();
                // Refresh the list
                RecipeIngrediantAmountList = recipeData.GetAllSelectedRecipeIngrediantAmount(RecipeID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if its possible to add the new ingredient
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
                if (addIngredientAmountToRecipe.txtQuantity.Text.Length < 1)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Unesite broj.", "Pogresan unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (addIngredientAmountToRecipe.txtQuantity.Text.ToString() == "0" || addIngredientAmountToRecipe.txtQuantity.Text.ToString() == "00")
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Kolicina mora biti veca od nule.", "Pogresan unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                IngredientAmount.RecipeID = RecipeID;
                IngredientAmount.IngredientID = Ingredient.IngredientID;
                recipeData.AddIngredientAmount(IngredientAmount);
                addIngredientAmountToRecipe.Close();
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
            if (addIngredientAmountToRecipe.txtQuantity.Text.Trim() == "" ||
                addIngredientAmountToRecipe.txtQuantity.Text.Trim() == "0")
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
                addIngredientAmountToRecipe.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

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
            tblIngredient ing = ingrediantsData.FindIngredient(IngredientAmount.IngredientID);
            try
            {
                MessageBoxResult dialogDelete = Xceed.Wpf.Toolkit.MessageBox.Show($"Da li zelite da obrisete ovaj sastojak iz liste?\n\nSastojak: {ing.IngredientName}", "Obrisi sastojak", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (dialogDelete == MessageBoxResult.Yes)
                {
                    if (IngredientAmount != null)
                    {
                        recipeData.DeleteIngredientAmount(IngredientAmount.IngredientAmountID);
                        RecipeIngrediantAmountList = recipeData.GetAllSelectedRecipeIngrediantAmount(RecipeID);
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
        public bool CanDeleteExecute()
        {
            if (IngredientAmount == null)
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
            MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Da li odustajete od dodavanja sastojaka\nCeo proces kreiranja recepta ce vam biti ignorisan", "Odustani", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (dialog == MessageBoxResult.Yes)
            {
                recipeData.DeleteRecipe(RecipeID);
                //addIngredientToRecipe.Close();
                AllRecipesViewModel.isRecipeNotUpdated = true;
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
            MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Da li zelite da sacuvate recept?", "Zavrsetak", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (dialog == MessageBoxResult.Yes)
            {
                //addIngredientToRecipe.Close();
            }
            else
            {
                recipeData.DeleteRecipe(RecipeID);
            }
        }

        /// <summary>
        /// Checks if its possible to press the button
        /// </summary>
        /// <returns></returns>
        private bool CanAddToRecipeExecute()
        {
            if (RecipeIngrediantAmountList.Count == 0)
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
