using CakeRecipes.Models;
using CakeRecipes.ViewModel;
using System;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AddIngredientAmountToRecipe.xaml
    /// </summary>
    public partial class AddIngredientAmountToRecipe : Window
    {
        public AddIngredientAmountToRecipe(tblIngredient ingredient, int RecipeID)
        {
            InitializeComponent();
            this.DataContext = new AddIngredientToRecipeViewModel(this, ingredient, RecipeID);
        }

        public AddIngredientAmountToRecipe(tblIngredient ingredient)
        {
            InitializeComponent();
            this.DataContext = new AddIngredientToBasketViewModel(this, ingredient);
        }

        /// <summary>
        /// A method that allows only numbers to be entered
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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

        private void PreviewNumberInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberAllowed(e.Text);
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
    }
}
