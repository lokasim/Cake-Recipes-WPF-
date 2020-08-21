using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CakeRecipes.ViewModel
{
    public class ItemMenu
    {
        public ItemMenu(string header, List<Subitem> subItems, PackIconKind icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
        }

        public ItemMenu(string header, UserControl screen, PackIconKind icon)
        {
            Header = header;
            Screen = screen;
            Icon = icon;
        }

        public string Header { get; private set; }
        public PackIconKind Icon { get; private set; }
        public List<Subitem> SubItems { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
