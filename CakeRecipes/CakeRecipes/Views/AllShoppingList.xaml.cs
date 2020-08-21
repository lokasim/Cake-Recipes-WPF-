using CakeRecipes.ViewModel;
using System.Windows.Controls;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AllShoppingList.xaml
    /// </summary>
    public partial class AllShoppingList : UserControl
    {
        public AllShoppingList()
        {
            InitializeComponent();
            this.DataContext = new AllShoppingListViewModel(this);
        }
    }
}
