using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CakeRecipes.Command;
using CakeRecipes.Models;
using CakeRecipes.Services;
using CakeRecipes.Views;

namespace CakeRecipes.ViewModel
{
    class AddIngredientMenuViewModel : ViewModelBase
    {
        private AddIngredientMenu addIngredientMenu;
        IngredientService ingrediantsData = new IngredientService();

        #region Constructor
        /// <summary>
        /// Opens the Add ingredient to recipe window
        /// </summary>
        /// <param name="addIngrediwntOpen">Window that we open</param>

        public AddIngredientMenuViewModel(AddIngredientMenu addIngredientMenu)
        {
            this.addIngredientMenu = addIngredientMenu;
            ingredient = new tblIngredient();
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
                AddIngredient addIngredient = new AddIngredient();
                addIngredient.ShowDialog();
                IngredientList = ingrediantsData.GetAllIngredients();
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
                return true;
            
        }
        #endregion
    }
}
