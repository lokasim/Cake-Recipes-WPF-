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
        private ICommand sortByNameAsc;
        public ICommand SortByNameAsc
        {
            get
            {
                if (sortByNameAsc == null)
                {
                    sortByNameAsc = new RelayCommand(param => SortByNameAscExecute(), param => CanSortByNameAscExecute());
                }
                return sortByNameAsc;
            }
        }

        /// <summary>
        /// Method for sorting by name ascending
        /// </summary>
        public void SortByNameAscExecute()
        {

            if (AllRecipesWindow.filteredList.Count != 0)
            {
                allReciperWindow.DataGridOrder.ItemsSource = AllRecipesWindow.filteredList.OrderBy(x => x.RecipeName).ToList();

            }
            else
            {
                allReciperWindow.DataGridOrder.ItemsSource = recipeData.GetAllRecipes().OrderBy(x => x.RecipeName).ToList();
            }
        }

        /// <summary>
        /// Checks if its possible to press the sort button
        /// </summary>
        /// <returns></returns>
        public bool CanSortByNameAscExecute()
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
        private ICommand sortByNameDesc;
        public ICommand SortByNameDesc
        {
            get
            {
                if (sortByNameDesc == null)
                {
                    sortByNameDesc = new RelayCommand(param => SortByNameDescExecute(), param => CanSortByNameDescExecute());
                }
                return sortByNameDesc;
            }
        }

        /// <summary>
        /// Method for sorting by name descending
        /// </summary>
        public void SortByNameDescExecute()
        {
            //RecipeList = AllRecipesWindow.filteredList.OrderByDescending(x => x.RecipeName).ToList();
            if(AllRecipesWindow.filteredList.Count != 0)
            {
                allReciperWindow.DataGridOrder.ItemsSource = AllRecipesWindow.filteredList.OrderByDescending(x => x.RecipeName).ToList();
            }
            else
            {
                allReciperWindow.DataGridOrder.ItemsSource = recipeData.GetAllRecipes().OrderByDescending(x => x.RecipeName).ToList();

            }

            //allReciperWindow.DataGridOrder.ItemsSource = RecipeList;
            //AllRecipesWindow.filteredList = RecipeList;
        }

        /// <summary>
        /// Checks if its possible to press the sort button
        /// </summary>
        /// <returns></returns>
        public bool CanSortByNameDescExecute()
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
        private ICommand sortByDateAsc;
        public ICommand SortByDateAsc
        {
            get
            {
                if (sortByDateAsc == null)
                {
                    sortByDateAsc = new RelayCommand(param => SortByDateAscExecute(), param => CanSortByDateAscExecute());
                }
                return sortByDateAsc;
            }
        }

        /// <summary>
        /// Method for sorting by date ascending
        /// </summary>
        public void SortByDateAscExecute()
        {
            if (AllRecipesWindow.filteredList.Count != 0)
            {
                allReciperWindow.DataGridOrder.ItemsSource = AllRecipesWindow.filteredList.OrderBy(x => x.CreationDate).ToList();

            }
            else
            {
                allReciperWindow.DataGridOrder.ItemsSource = recipeData.GetAllRecipes().OrderBy(x => x.CreationDate).ToList();
            }
            
        }

        /// <summary>
        /// Checks if its possible to press the sort button
        /// </summary>
        /// <returns></returns>
        public bool CanSortByDateAscExecute()
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
        private ICommand sortByDateDesc;
        public ICommand SortByDateDesc
        {
            get
            {
                if (sortByDateDesc == null)
                {
                    sortByDateDesc = new RelayCommand(param => SortByDateDescExecute(), param => CanSortByDateDescExecute());
                }
                return sortByDateDesc;
            }
        }

        /// <summary>
        /// Method for sorting by date descending
        /// </summary>
        public void SortByDateDescExecute()
        {
            if (AllRecipesWindow.filteredList.Count != 0)
            {
                allReciperWindow.DataGridOrder.ItemsSource = AllRecipesWindow.filteredList.OrderByDescending(x => x.CreationDate).ToList();

            }
            else
            {
                allReciperWindow.DataGridOrder.ItemsSource = recipeData.GetAllRecipes().OrderByDescending(x => x.CreationDate).ToList();

            }
        }

        /// <summary>
        /// Checks if its possible to press the sort button
        /// </summary>
        /// <returns></returns>
        public bool CanSortByDateDescExecute()
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
        private ICommand sortByIngredientAmountAsc;
        public ICommand SortByIngredientAmountAsc
        {
            get
            {
                if (sortByIngredientAmountAsc == null)
                {
                    sortByIngredientAmountAsc = new RelayCommand(param => SortByIngredientAmountAscExecute(), param => CanSortByIngredientAmountAscExecute());
                }
                return sortByIngredientAmountAsc;
            }
        }

        /// <summary>
        /// Method for sorting by ingredient amount ascending
        /// </summary>
        public void SortByIngredientAmountAscExecute()
        {
            allReciperWindow.DataGridOrder.ItemsSource = recipeData.SortByAmountAsecnding().ToList();
        }

        /// <summary>
        /// Checks if its possible to press the sort button
        /// </summary>
        /// <returns></returns>
        public bool CanSortByIngredientAmountAscExecute()
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
        private ICommand sortByIngredientAmountDesc;
        public ICommand SortByIngredientAmountDesc
        {
            get
            {
                if (sortByIngredientAmountDesc == null)
                {
                    sortByIngredientAmountDesc = new RelayCommand(param => SortByIngredientAmountDescExecute(), param => CanSortByIngredientAmountDescExecute());
                }
                return sortByIngredientAmountDesc;
            }
        }

        /// <summary>
        /// Method for sorting by ingredient amount descending
        /// </summary>
        public void SortByIngredientAmountDescExecute()
        {
            allReciperWindow.DataGridOrder.ItemsSource = recipeData.SortByAmountDescending().ToList();
        }

        /// <summary>
        /// Checks if its possible to press the sort button
        /// </summary>
        /// <returns></returns>
        public bool CanSortByIngredientAmountDescExecute()
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
                        allReciperWindow.DataGridOrder.ItemsSource = RecipeList;
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
                        else
                        {
                            // Save recipe changes
                            recipeData.AddRecipe(Recipe);
                        }

                        RecipeList = recipeData.GetAllRecipes().ToList();
                        allReciperWindow.DataGridOrder.ItemsSource = RecipeList;
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
                allReciperWindow.DataGridOrder.ItemsSource = RecipeList;
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
        /// Add Recipe button
        /// </summary>
        private ICommand addIngredientBtn;
        public ICommand AddIngredientBtn
        {
            get
            {
                if (addIngredientBtn == null)
                {
                    addIngredientBtn = new RelayCommand(param => AddIngredientBtnExecute(), param => CanAddIngredientBtnExecute());
                }
                return addIngredientBtn;
            }
        }

        /// <summary>
        /// Method for adding the selected item from the list
        /// </summary>
        public void AddIngredientBtnExecute()
        {
            try
            {
                //AddRecipe addRecipeWindow = new AddRecipe();
                //addRecipeWindow.ShowDialog();
                //RecipeList = recipeData.GetAllRecipes().ToList();
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Desilo se nešto nepredviđeno...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if its possible to press the add button
        /// </summary>
        /// <returns></returns>
        public bool CanAddIngredientBtnExecute()
        {
                return true;
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
            allReciperWindow.NazivRecepta.Text = Recipe.RecipeName.ToString();
            allReciperWindow.Autor.Text = Recipe.Changed.ToString();
            allReciperWindow.Datum.Text = Recipe.CreationDate.ToString("dd.MM.yyyy").ToString();
            allReciperWindow.Tip.Text = Recipe.RecipeType.ToString();
            allReciperWindow.Opis.Text = Recipe.RecipeDescription.ToString();
            allReciperWindow.Sastojci.Text = ingredientText.ToString() + Environment.NewLine;
                
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
