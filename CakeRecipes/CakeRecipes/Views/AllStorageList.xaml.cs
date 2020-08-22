using CakeRecipes.ViewModel;
using System.Windows.Controls;

namespace CakeRecipes.Views
{
    /// <summary>
    /// Interaction logic for AllShoppingList.xaml
    /// </summary>
    public partial class AllStorageList : UserControl
    {
        public AllStorageList()
        {
            InitializeComponent();
            this.DataContext = new AllStorageViewModel(this);
            this.Name = "AllStorageList";
        }
    }
}

