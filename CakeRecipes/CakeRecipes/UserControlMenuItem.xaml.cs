using CakeRecipes.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CakeRecipes
{
    /// <summary>
    /// Interaction logic for UserControlMenuItem.xaml
    /// </summary>
    public partial class UserControlMenuItem : UserControl
    {
        MainWindow _context;
        private bool mouseClicked;

        public UserControlMenuItem(ItemMenu itemMenu, MainWindow context)
        {
            InitializeComponent();

            _context = context;

            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;

            this.DataContext = itemMenu;
        }

        /// <summary>
        /// The method that determines which menu is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mouseClicked)
            {
                if (e.AddedItems.Count > 0)
                    _context.SwitchScreen(((Subitem)((ListView)sender).SelectedItem).Screen);
            }
        }
        private void ListViewMenu_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseClicked = true;
            if (ListViewMenu.SelectedItem != null)
            {
                _context.SwitchScreen(((Subitem)((ListView)sender).SelectedItem).Screen);
            }
        }
    }
}
