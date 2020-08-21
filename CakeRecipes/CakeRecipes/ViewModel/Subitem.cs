using System.Windows.Controls;

namespace CakeRecipes.ViewModel
{
    public class Subitem
    {
        /// <summary>
        /// Menu item
        /// </summary>
        /// <param name="name">Name menu item</param>
        /// <param name="screen">UserControl Menu Item</param>
        public Subitem(string name, UserControl screen = null)
        {
            Name = name;
            Screen = screen;
        }
        public string Name { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
