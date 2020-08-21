using CakeRecipes.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AddIngredientToRecipe.xaml
    /// </summary>
    public partial class AddIngredientToRecipe : UserControl
    {
        public AddIngredientToRecipe(int RecipeID)
        {
            InitializeComponent();
            this.DataContext = new AddIngredientToRecipeViewModel(this, RecipeID);
            this.Language = XmlLanguage.GetLanguage("sr-SR");
        }
    }
}
