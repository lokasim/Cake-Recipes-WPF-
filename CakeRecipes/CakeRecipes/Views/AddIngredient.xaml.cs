using CakeRecipes.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AddIngredient.xaml
    /// </summary>
    public partial class AddIngredient : Window
    {
        public AddIngredient()
        {
            InitializeComponent();
            this.Name = "AddIngredient";
            this.DataContext = new AddIngredientViewModel(this);
            this.Language = XmlLanguage.GetLanguage("sr-SR");
            lblNaslov.Content = "Add Ingredient";
        }

        private void DragMe(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {

                // throw;
            }
        }
    }
}
