using CakeRecipes.Services;
using CakeRecipes.ViewModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AllRecipes.xaml
    /// </summary>
    public partial class AllRecipesWindow : UserControl
    {
        public AllRecipesWindow()
        {
            InitializeComponent();
            this.DataContext = new AllRecipesViewModel(this);
            if(LoginViewModel.usersLogin == true)
            {
                btnDeleteRecipe.Visibility = System.Windows.Visibility.Collapsed;
                Thickness marginThickness = btnEditRecipe.Margin;
                btnEditRecipe.Margin = new Thickness(30,0,30,15);
                Thickness marginThickness1 = btnAddRecipe.Margin;
                btnAddRecipe.Margin = new Thickness(30, 0, 140, 15);

            }

        }
    }
}
