using CakeRecipes.Models;
using CakeRecipes.Services;
using CakeRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AllRecipes.xaml
    /// </summary>
    public partial class AllRecipesWindow : UserControl
    {
        public static List<tblRecipe> filteredList = new List<tblRecipe>();
        public List<tblRecipe> items = new List<tblRecipe>();
        public CollectionView view;
        public List<tblRecipe> filteredRecipes;

        public AllRecipesWindow()
        {
            InitializeComponent();
            this.DataContext = new AllRecipesViewModel(this);

            RecipeService recipeService = new RecipeService();

            items = recipeService.GetAllRecipes();
            DataGridOrder.ItemsSource = items;
            view = (CollectionView)CollectionViewSource.GetDefaultView(DataGridOrder.ItemsSource);
            view.Filter = UserFilter;
            filteredList = new List<tblRecipe>();
            filteredRecipes = new List<tblRecipe>();

            if (LoginViewModel.usersLogin == true)
            {
                btnDeleteRecipe.Visibility = System.Windows.Visibility.Collapsed;
                Thickness marginThickness = btnEditRecipe.Margin;
                btnEditRecipe.Margin = new Thickness(30,0,30,15);
                Thickness marginThickness1 = btnAddRecipe.Margin;
                btnAddRecipe.Margin = new Thickness(30, 0, 140, 15);
            }
        }

        private void TxtBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
            if (e.Key == Key.Space)
            {
                SystemSounds.Beep.Play();
            }
        }

        private Boolean TextAllowed(String s)
        {
            foreach (Char c in s.ToCharArray())
            {
                if (Char.IsLetter(c) || Char.IsControl(c))
                {

                    continue;
                }
                else
                {
                    SystemSounds.Beep.Play();
                    return false;
                }
            }
            return true;
        }

        // zabranjuje da se kopiraju brojevi u polje
        private void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            String s = (String)e.DataObject.GetData(typeof(String));
            if (!TextAllowed(s)) e.CancelCommand();
        }

        //only numbers
        private void PreviewNumberInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberAllowed(e.Text);
        }

        private Boolean NumberAllowed(String s)
        {
            foreach (Char c in s.ToCharArray())
            {
                if (Char.IsDigit(c) || Char.IsControl(c))
                {

                    continue;
                }
                else
                {
                    SystemSounds.Beep.Play();
                    return false;
                }
            }
            return true;
        }

        private bool UserFilter(object item)
        {

            try
            {
                if (!String.IsNullOrEmpty(txtRecipeName.Text) && txtRecipeName.Text.Length >= 3 && txtRecipeName.IsEnabled == true && cbxRecipeTypes.IsEnabled == false)
                {
                    return ((item as tblRecipe).RecipeName.IndexOf(txtRecipeName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                }
                else if (txtRecipeName.Text.Length < 3 && cbxRecipeTypes.SelectedValue != null && !String.IsNullOrEmpty(cbxRecipeTypes.SelectedValue.ToString()) && txtRecipeName.IsEnabled == true && cbxRecipeTypes.IsEnabled == true)
                {
                    return ((item as tblRecipe).RecipeType.IndexOf(cbxRecipeTypes.SelectedValue.ToString(), StringComparison.OrdinalIgnoreCase) >= 0);
                }
                else if (!String.IsNullOrEmpty(txtRecipeName.Text) && txtRecipeName.Text.Length >= 3 && cbxRecipeTypes.SelectedValue != null && !String.IsNullOrEmpty(cbxRecipeTypes.SelectedValue.ToString()) && txtRecipeName.IsEnabled == true && cbxRecipeTypes.IsEnabled == true)
                {
                    return ((item as tblRecipe).RecipeType.IndexOf(cbxRecipeTypes.SelectedValue.ToString(), StringComparison.OrdinalIgnoreCase) >= 0 && ((item as tblRecipe).RecipeName.IndexOf(txtRecipeName.Text, StringComparison.OrdinalIgnoreCase) >= 0));
                }
                else if (cbxRecipeTypes.SelectedValue != null && !String.IsNullOrEmpty(cbxRecipeTypes.SelectedValue.ToString()) && txtRecipeName.IsEnabled == false && cbxRecipeTypes.IsEnabled == true)
                {
                    return ((item as tblRecipe).RecipeType.IndexOf(cbxRecipeTypes.SelectedValue.ToString(), StringComparison.OrdinalIgnoreCase) >= 0);
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                //Xceed.Wpf.Toolkit.MessageBox.Show("Upsss! Desilo se nešto nepredviđeno pokušajte pretražiti po drugom kriterijumu...","Greška u pretrazi.");
                return true;
            }
        }

        private void TxtRecipeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            RecipeService recipeService = new RecipeService();

            items = recipeService.GetAllRecipes();

            CollectionViewSource.GetDefaultView(DataGridOrder.ItemsSource).Refresh();
                filteredList = items.Where(i => view.Filter(i)).ToList();
            DataGridOrder.ItemsSource = items.Where(i => view.Filter(i)).ToList();

        }
        

        private void CheckName_Click(object sender, RoutedEventArgs e)
        {
            RecipeService recipeService = new RecipeService();

            items = recipeService.GetAllRecipes();
            view.Filter = UserFilter;
            CollectionViewSource.GetDefaultView(DataGridOrder.ItemsSource).Refresh();
            filteredList = items.Where(i => view.Filter(i)).ToList();
            DataGridOrder.ItemsSource = items.Where(i => view.Filter(i)).ToList();
        }

        private void CbxRecipeTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecipeService recipeService = new RecipeService();

            items = recipeService.GetAllRecipes();
            ComboBox cbx = (ComboBox)sender;
            string val = String.Empty;
            if(cbx.SelectedValue == null)
            {
                val = cbxRecipeTypes.SelectionBoxItem.ToString();
            }
            else
            {
                val = cbx.SelectedValue.ToString();
            }

            if(val!= null)
            {
                CollectionViewSource.GetDefaultView(DataGridOrder.ItemsSource).Refresh();
                filteredList = items.Where(i => view.Filter(i)).ToList();
                DataGridOrder.ItemsSource = items.Where(i => view.Filter(i)).ToList();
                
            }

            
        }

        private void CheckType_Click(object sender, RoutedEventArgs e)
        {
            RecipeService recipeService = new RecipeService();

            items = recipeService.GetAllRecipes();
            if (cbxRecipeTypes.Text.ToString() != "")
            {
                view.Filter = UserFilter;
                CollectionViewSource.GetDefaultView(DataGridOrder.ItemsSource).Refresh();
                filteredList = items.Where(i => view.Filter(i)).ToList();
                DataGridOrder.ItemsSource = items.Where(i => view.Filter(i)).ToList();
            }
        }
    }
}
