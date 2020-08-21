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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AddIngredientMenu.xaml
    /// </summary>
    public partial class AddIngredientMenu : UserControl
    {
        public AddIngredientMenu()
        {
            InitializeComponent();
            this.DataContext = new AddIngredientMenuViewModel(this);
            this.Language = XmlLanguage.GetLanguage("sr-SR");
        }
    }
}
