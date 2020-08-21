using CakeRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public bool loggedIn;

        public Login()
        {
            InitializeComponent();
            btnPrijavi.IsEnabled = false;
            btnRegistruj.IsEnabled = false;
            NameTextBox.Focus();
            this.DataContext = new LoginViewModel(this);
            this.Language = XmlLanguage.GetLanguage("sr-SR");
        }

        public async void SnackError()
        {
            SnackErrorSNC.IsActive = true;
            await Task.Delay(3000);
            SnackErrorSNC.IsActive = false;
        }

        public bool korisnik;
        public bool lozinka;
        public bool nameSurname;

        private void KorekcijaImena(object sender, TextChangedEventArgs e)
        {
            if (NameTextBox.Text.Length <= 4)
            {
                MinUsername.Visibility = Visibility.Visible;
                NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                NameTextBox.Foreground = new SolidColorBrush(Colors.Red);
                korisnik = false;
            }
            else
            {
                MinUsername.Visibility = Visibility.Collapsed;

                NameTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
                NameTextBox.Foreground = new SolidColorBrush(Colors.Black);
                korisnik = true;
            }
            Prijavi();
        }

        //Correction Password
        private void KorekcijaLozinke(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password.Length <= 4)
            {
                MinPassword.Visibility = Visibility.Visible;
                passwordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                passwordBox.Foreground = new SolidColorBrush(Colors.Red);
                lozinka = false;
            }
            else
            {
                MinPassword.Visibility = Visibility.Collapsed;

                passwordBox.BorderBrush = new SolidColorBrush(Colors.Green);
                passwordBox.Foreground = new SolidColorBrush(Colors.Black);
                lozinka = true;
            }
            Prijavi();
        }

        private void Prijavi()
        {
            if (lozinka == true && korisnik == true)
            {
                btnPrijavi.IsEnabled = true;
            }
            else
            {
                btnPrijavi.IsEnabled = false;
            }
        }

        private void KorekcijaImenaPrezimena(object sender, TextChangedEventArgs e)
        {
            if (NameTextBox.Text.Length <= 3)
            {
                MinNameLastname.Visibility = Visibility.Visible;
                nameSurnameUser.BorderBrush = new SolidColorBrush(Colors.Red);
                nameSurnameUser.Foreground = new SolidColorBrush(Colors.Red);
                nameSurname = false;
            }
            else
            {
                MinNameLastname.Visibility = Visibility.Collapsed;
                nameSurnameUser.BorderBrush = new SolidColorBrush(Colors.Green);
                nameSurnameUser.Foreground = new SolidColorBrush(Colors.Black);
                nameSurname = true;
            }
            Registruj();
        }

        private void Registruj()
        {
            if (nameSurname == true)
            {
                btnRegistruj.IsEnabled = true;
            }
            else
            {
                btnRegistruj.IsEnabled = false;
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

            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void TxtBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
            if (e.Key == Key.Space)
            {
                SystemSounds.Beep.Play();
            }
        }

        private Boolean TextAllowedVelikaSlova(String s)
        {
            foreach (Char c in s.ToCharArray())
            {
                if (Char.IsLower(c) || Char.IsUpper(c) || Char.IsDigit(c) || Char.IsControl(c))
                {
                    loginFail.Visibility = Visibility.Collapsed;
                    tbCapsLock.Visibility = Visibility.Collapsed;
                    continue;
                }
                else
                {
                    tbCapsLock.Visibility = Visibility.Visible;
                    tbCapsLock.Text = "Dozvoljne je unos slova i brojeva";
                    SystemSounds.Beep.Play();
                    return false;
                }
            }
            return true;
        }

        //samo mala slova i brojevi
        private void PreviewTextInputHandlerVelika(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !TextAllowedVelikaSlova(e.Text);
        }

        private Boolean TextAllowed(String s)
        {
            foreach (Char c in s.ToCharArray())
            {
                if (Char.IsLetter(c) || Char.IsControl(c))
                {

                    continue;
                }
                else
                {
                    SystemSounds.Beep.Play();
                    return false;
                }
            }
            return true;
        }

        private void PreviewTextInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !TextAllowed(e.Text);
        }

        // banned pasting value
        private void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            String s = (String)e.DataObject.GetData(typeof(String));
            if (!TextAllowed(s)) e.CancelCommand();
        }

        private Boolean NumberAllowed(String s)
        {
            foreach (Char c in s.ToCharArray())
            {
                if (Char.IsDigit(c) || Char.IsControl(c))
                {

                    continue;
                }
                else
                {
                    SystemSounds.Beep.Play();
                    return false;
                }
            }
            return true;
        }

        //only numbers
        private void PreviewNumberInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberAllowed(e.Text);
        }
    }
}