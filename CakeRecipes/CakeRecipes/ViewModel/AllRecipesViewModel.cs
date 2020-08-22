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
            SortByAmountBtn = "Broj Sastojka";
            SortByDateBtn = "Datum";
            SortByNameBtn = "Naziv";
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

        /// <summary>
        /// Sort button name
        /// </summary>
        private string sortByAmountBtn;
        public string SortByAmountBtn
        {
            get
            {
                return sortByAmountBtn;
            }
            set
            {
                sortByAmountBtn = value;
                OnPropertyChanged("SortByAmountBtn");
            }
        }

        /// <summary>
        /// Sort button name
        /// </summary>
        private string sortByNameBtn;
        public string SortByNameBtn
        {
            get
            {
                return sortByNameBtn;
            }
            set
            {
                sortByNameBtn = value;
                OnPropertyChanged("SortByNameBtn");
            }
        }

        /// <summary>
        /// Sort button name
        /// </summary>
        private string sortByDateBtn;
        public string SortByDateBtn
        {
            get
            {
                return sortByDateBtn;
            }
            set
            {
                sortByDateBtn = value;
                OnPropertyChanged("SortByDateBtn");
            }
        }

        /// <summary>
        /// Number of people
        /// </summary>
        private int newNoPeople;
        public int NewNoPeople
        {
            get
            {
                return newNoPeople;
            }
            set
            {
                newNoPeople = value;
                OnPropertyChanged("NewNoPeople");
            }
        }

        /// <summary>
        /// Recipe text
        /// </summary>
        private string recipeText;
        public string RecipeText
        {
            get
            {
                return recipeText;
            }
            set
            {
                recipeText = value;
                OnPropertyChanged("RecipeText");
            }
        }

        /// <summary>
        /// Info text
        /// </summary>
        private string infoText;
        public string InfoText
        {
            get
            {
                return infoText;
            }
            set
            {
                infoText = value;
                OnPropertyChanged("InfoText");
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Sort Recipe button
        /// </summary>
        private ICommand sortByName;
        public ICommand SortByName
        {
            get
            {
                if (sortByName == null)
                {
                    sortByName = new RelayCommand(param => SortByNameExecute(), param => CanSortByNameExecute());
                }
                return sortByName;
            }
        }

        private static bool orderNameAsc = false;
        /// <summary>
        /// Method for sorting by name
        /// </summary>
        public void SortByNameExecute()
        {
         
            if (orderNameAsc == true)
            {
                orderNameAsc = false;
                RecipeList = recipeData.GetAllRecipes().OrderByDescending(x => x.RecipeName).ToList();
                SortByNameBtn = "Naziv desc";
            }
            else
            {
                orderNameAsc = true;
                RecipeList = recipeData.GetAllRecipes().OrderBy(x => x.RecipeName).ToList();
                SortByNameBtn = "Naziv asc";
            }
        }

        /// <summary>
        /// Checks if its possible to press the sort button
        /// </summary>
        /// <returns></returns>
        public bool CanSortByNameExecute()
        {
            if (RecipeList.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Sort Recipe button
        /// </summary>
        private ICommand sortByDate;
        public ICommand SortByDate
        {
            get
            {
                if (sortByDate == null)
                {
                    sortByDate = new RelayCommand(param => SortByDateExecute(), param => CanSortByDateExecute());
                }
                return sortByDate;
            }
        }

        private static bool orderDateAsc = false;
        /// <summary>
        /// Method for sorting by name
        /// </summary>
        public void SortByDateExecute()
        {

            if (orderDateAsc == true)
            {
                orderDateAsc = false;
                RecipeList = recipeData.GetAllRecipes().OrderByDescending(x => x.CreationDate).ToList();
                SortByDateBtn = "Datum desc";
            }
            else
            {
                orderDateAsc = true;
                RecipeList = recipeData.GetAllRecipes().OrderBy(x => x.CreationDate).ToList();
                SortByDateBtn = "Datum asc";
            }
        }

        /// <summary>
        /// Checks if its possible to press the sort button
        /// </summary>
        /// <returns></returns>
        public bool CanSortByDateExecute()
        {
            if (RecipeList.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Sort Recipe button
        /// </summary>
        private ICommand sortByIngredientAmount;
        public ICommand SortByIngredientAmount
        {
            get
            {
                if (sortByIngredientAmount == null)
                {
                    sortByIngredientAmount = new RelayCommand(param => SortByIngredientAmountExecute(), param => CanSortByIngredientAmountExecute());
                }
                return sortByIngredientAmount;
            }
        }

        public static bool orderAmountAsc = false;
        /// <summary>
        /// Method for sorting by ingredient amount
        /// </summary>
        public void SortByIngredientAmountExecute()
        {

            if (orderAmountAsc == true)
            {
                orderAmountAsc = false;
                RecipeList = recipeData.SortByAmountAsecnding().ToList();
                SortByAmountBtn = "Broj desc";
            }
            else
            {
                orderAmountAsc = true;
                RecipeList = recipeData.SortByAmountDescending().ToList();
                SortByAmountBtn = "Broj asc";
            }
        }

        /// <summary>
        /// Checks if its possible to press the sort button
        /// </summary>
        /// <returns></returns>
        public bool CanSortByIngredientAmountExecute()
        {
            if (RecipeList.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


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

        /// <summary>
        /// Preview Recipe button
        /// </summary>
        private ICommand previewRecipe;
        public ICommand PreviewRecipe
        {
            get
            {
                if (previewRecipe == null)
                {
                    previewRecipe = new RelayCommand(param => PreviewRecipeExecute(), param => CanPreviewRecipeExecute());
                }
                return previewRecipe;
            }
        }

        /// <summary>
        /// Method for previewing a recipe
        /// </summary>
        public void PreviewRecipeExecute()
        {
            string ingredientText = "";

            foreach (var item in recipeData.CountRecipeValue(Recipe, NewNoPeople))
            {
                ingredientText += item.Key + ": " + item.Value.ToString() + Environment.NewLine;
            }

            RecipeText =
                "Naziv recepta: " + Recipe.RecipeName + "\t\tAutor: " + Recipe.Changed + Environment.NewLine +
                "Tip recepta: " + Recipe.RecipeType + "\t\tDatum: " + Recipe.CreationDate.ToString("dd.MM.yyyy") + Environment.NewLine +
                "Opis: " + Recipe.RecipeDescription + Environment.NewLine + Environment.NewLine +
                "Sastojci: " + Environment.NewLine +
                ingredientText;
        }

        /// <summary>
        /// Checks if its possible to press the preview button
        /// </summary>
        /// <returns></returns>
        public bool CanPreviewRecipeExecute()
        {
            if (NewNoPeople < 1 && Recipe == null)
            {
                InfoText = "Da biste prikazali recept, potrebno je da" + Environment.NewLine + "unesete broj osoba i selektujete recept iz tabele.";
                return false;
            }
            else if (NewNoPeople > 0 && Recipe == null)
            {
                InfoText = "Da biste prikazali recept, potrebno je da" + Environment.NewLine + "selektujete recept iz tabele.";
                return  false;
            }
            else if (NewNoPeople < 1 && Recipe != null)
            {
                InfoText = "Da biste prikazali recept, potrebno je da" + Environment.NewLine + "unesete broj osoba veceg od nula.";
                return false;
            }
            else
            {
                InfoText = "Vrednost je uvek zaokruzena na veci broj.";
                return true;
            }
        }
        #endregion
    }
}
