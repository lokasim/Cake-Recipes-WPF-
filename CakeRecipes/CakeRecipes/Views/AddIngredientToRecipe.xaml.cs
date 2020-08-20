using CakeRecipes.ViewModel;
using System.Windows;
using System.Windows.Markup;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AddIngredientToRecipe.xaml
    /// </summary>
    public partial class AddIngredientToRecipe : Window
    {
        public AddIngredientToRecipe()
        {
            InitializeComponent();
            this.DataContext = new AddIngredientToRecipeViewModel(this);
            this.Language = XmlLanguage.GetLanguage("sr-SR");
        }
    }
}
