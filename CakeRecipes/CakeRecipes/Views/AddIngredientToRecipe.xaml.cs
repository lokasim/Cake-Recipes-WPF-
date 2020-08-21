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
        public AddIngredientToRecipe(int RecipeID)
        {
            InitializeComponent();
            this.DataContext = new AddIngredientToRecipeViewModel(this, RecipeID);
            this.Language = XmlLanguage.GetLanguage("sr-SR");
        }

        public AddIngredientToRecipe()
        {
            InitializeComponent();
            this.DataContext = new AddIngredientToBasketViewModel(this);
            this.Language = XmlLanguage.GetLanguage("sr-SR");
        }
    }
}
