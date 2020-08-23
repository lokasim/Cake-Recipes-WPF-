using CakeRecipes.ViewModel;
using CakeRecipes.Views;
using MaterialDesignThemes.Wpf;
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
using System.Windows.Threading;

namespace CakeRecipes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(this);
            this.Language = XmlLanguage.GetLanguage("en-GB");

            Login login = new Login();
            if (login.loggedIn == false)
            {
                login.ShowDialog();
            }
            else
            {
                login.Close();
            }

            //Employee menu
            if (LoginViewModel.adminLogin == true)
            {
                lblPrezime.Content = "Administrator".ToString();
                lblIme.Content = "".ToString(); ;
                var menuOrders = new List<Subitem>
                    {
                        //new Subitem("Napisi novi recept", new AddRecipe()),
                        new Subitem("Svi Recepti", new AllRecipesWindow()),
                    };
                var item1 = new ItemMenu("Recepti", menuOrders, PackIconKind.Pizzeria);

                var menuShopping = new List<Subitem>
                    {
                        new Subitem("Sve Shopping Liste", new AllShoppingList()),
                    };
                var item2 = new ItemMenu("Shopping Lista", menuShopping, PackIconKind.Pizzeria);


                var menuIngredient = new List<Subitem>
                    {
                        new Subitem("Lista sastojaka", new AddIngredientMenu()),
                    };
                var item22 = new ItemMenu("Sastojci", menuIngredient, PackIconKind.Cookie);

                var menuStorage = new List<Subitem>
                    {
                        new Subitem("Skladiste Sastojaka", new AllStorageList()),
                    };
                var item33 = new ItemMenu("Skladiste", menuStorage, PackIconKind.Cookie);

                var item50 = new ItemMenu("Menu", new UserControl(), PackIconKind.Pizza);

                Menu.Children.Add(new UserControlMenuItem(item50, this));
                Menu.Children.Add(new UserControlMenuItem(item1, this));
                Menu.Children.Add(new UserControlMenuItem(item2, this));
                Menu.Children.Add(new UserControlMenuItem(item22, this));
                Menu.Children.Add(new UserControlMenuItem(item33, this));
            }

            //Guest menu
            if (LoginViewModel.usersLogin == true)
            {
                lblPrezime.Content = LoggedGuest.NameSurname;

                //if (LoggedGuest.NameSurname != null || LoggedGuest.NameSurname!= "")
                //{
                //    PrintMessage();
                //}

                var menuOrders = new List<Subitem>
                    {
                        //new Subitem("Napisi novi recept", new AddRecipe()),
                        new Subitem("Svi Recepti", new AllRecipesWindow()),
                    };
                var item1 = new ItemMenu("Recepti", menuOrders, PackIconKind.Pizzeria);

                var menuShopping = new List<Subitem>
                    {
                        new Subitem("Sve Shopping Liste", new AllShoppingList()),
                    };
                var item2 = new ItemMenu("Shopping Lista", menuShopping, PackIconKind.Pizzeria);


                var menuIngredient = new List<Subitem>
                    {
                        new Subitem("Lista sastojaka", new AddIngredientMenu()),
                    };
                var item22 = new ItemMenu("Sastojci", menuIngredient, PackIconKind.Cookie);

                var menuStorage = new List<Subitem>
                    {
                        new Subitem("Skladiste Sastojaka", new AllStorageList()),
                    };
                var item33 = new ItemMenu("Skladiste", menuStorage, PackIconKind.Cookie);

                var item50 = new ItemMenu("Menu", new UserControl(), PackIconKind.Pizza);

                Menu.Children.Add(new UserControlMenuItem(item50, this));
                Menu.Children.Add(new UserControlMenuItem(item1, this));
                Menu.Children.Add(new UserControlMenuItem(item2, this));
                Menu.Children.Add(new UserControlMenuItem(item22, this));
                Menu.Children.Add(new UserControlMenuItem(item33, this));
            }

            //determines the current page length
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            //Current time
            Vreme();

        }
        /// <summary>
        /// Method to change the menu
        /// </summary>
        /// <param name="sender">Selected UserControl</param>
        public void SwitchScreen(object sender)
        {
            

            var screen = ((UserControl)sender);
            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(screen);

                //ArchivedOrder ao = StackPanelMain.FindName("ArchivedOrder") as ArchivedOrder;
                //ArchivedOrder ar = new ArchivedOrder();

                if (screen.Name == "AllStorageList")
                {
                    AllStorageList storageList = new AllStorageList();
                    StackPanelMain.Children.Clear();
                    StackPanelMain.Children.Add(storageList);
                }
                else if (screen.Name == "AllShoppingList")
                {
                    AllShoppingList shoppingList = new AllShoppingList();
                    StackPanelMain.Children.Clear();
                    StackPanelMain.Children.Add(shoppingList);
                }
                else
                {
                    StackPanelMain.Children.Clear();
                    StackPanelMain.Children.Add(screen);
                }

            }
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

        private void Vreme()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Dogadjaj;
            timer.Start();
        }

        /// <summary>
        /// method for printing the current time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dogadjaj(object sender, EventArgs e)
        {
            vr.Text = DateTime.Now.ToString(@"HH:mm:ss");
        }

        /// <summary>
        /// method for the application user to log out, 
        /// after which a new login form is displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel.usersLogin = false;
            LoginViewModel.adminLogin = false;
            this.Close();
            Login login = new Login
            {
                loggedIn = false
            };

            MainWindow main = new MainWindow();
            main.ShowDialog();
        }

        /// <summary>
        /// Exit application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dijalog = Xceed.Wpf.Toolkit.MessageBox.Show("Do you want to leave the program", "Exit app", MessageBoxButton.YesNo);

            if (dijalog == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Window enlargement method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Povecaj_prozor(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                PovecajProzor.ToolTip = "Restore Down";
                PovecajProzor1.Visibility = Visibility.Visible;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                PovecajProzor.ToolTip = "Maximize";
                PovecajProzor1.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Window reduction method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spusti_prozor(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Minimized;
            }
            else if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Minimized;
            }
        }
    }
}