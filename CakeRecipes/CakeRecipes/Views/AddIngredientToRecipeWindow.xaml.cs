using CakeRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AddIngredientToRecipeWindow.xaml
    /// </summary>
    public partial class AddIngredientToRecipeWindow : Window
    {
        public AddIngredientToRecipeWindow()
        {
            InitializeComponent();

            //AddIngredientToRecipe addIngredientToRecipe = new AddIngredientToRecipe();
            //UserControl screen = ((UserControl)addIngredientToRecipe);
            this.DataContext = new AddIngredientToRecipeWindowViewModel(this);
            //this.DataContext = new AddIngredientToBasketViewModel(this);
                this.Language = XmlLanguage.GetLanguage("sr-SR");
            
            //StackPanelMain.Children.Clear();
            //StackPanelMain.Children.Add(screen);
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
