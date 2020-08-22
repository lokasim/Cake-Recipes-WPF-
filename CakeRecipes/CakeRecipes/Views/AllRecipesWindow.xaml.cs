using CakeRecipes.Services;
using CakeRecipes.ViewModel;
using System;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
    }
}
