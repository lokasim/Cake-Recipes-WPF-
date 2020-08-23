using CakeRecipes.Command;
using CakeRecipes.Models;
using CakeRecipes.Services;
using CakeRecipes.Views;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CakeRecipes.ViewModel
{
    class LoginViewModel : ViewModelBase
    {
        private Login login;

        #region Properties
        public static bool adminLogin = false;
        public static bool usersLogin = false;

        private List<tblIngredient> ingredientList;
        public List<tblIngredient> IngredientList
        {
            get
            {
                return ingredientList;
            }
            set
            {
                ingredientList = value;
                OnPropertyChanged("IngredientList");
            }
        }

        private List<tblUser> userList;
        public List<tblUser> UserList
        {
            get
            {
                return userList;
            }
            set
            {
                userList = value;
                OnPropertyChanged("UserList");
            }
        }

        private tblUser user;
        public tblUser User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        private bool isUpdateUser;
        public bool IsUpdateUser
        {
            get
            {
                return isUpdateUser;
            }
            set
            {
                isUpdateUser = value;
            }
        }
        #endregion

        public LoginViewModel(Login login)
        {
            this.login = login;

            user = new tblUser();
        }

        #region Commands
        private ICommand exit;
        public ICommand Exit
        {
            get
            {
                if (exit == null)
                {
                    exit = new RelayCommand(param => ExitExecute(), param => CanExitExecute());
                }
                return exit;
            }
        }

        /// <summary>
        /// Exit application
        /// </summary>
        private void ExitExecute()
        {
            MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Da li želite napustiti aplikaciju?", "Izlaz", MessageBoxButton.YesNo);

            if (dialog == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        private bool CanExitExecute()
        {
            return true;
        }

        /// <summary>
        /// Login employee
        /// </summary>
        private ICommand loginRegister;
        public ICommand LoginRegister
        {
            get
            {
                if (loginRegister == null)
                {
                    loginRegister = new RelayCommand(param => LoginRegisterExecute(), param => CanLoginRegisterEExecute());
                }
                return loginRegister;
            }
        }

        public async void MessageIngredient()
        {

            await Task.Delay(3000);
            IngredientService ingrediantsData = new IngredientService();

            IngredientList = ingrediantsData.GetAllIngredients();

            if (IngredientList.Count < 1)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Kako bi sve funkcionalnosti aplikacije radile ispravno, potrebno je da unesete barem jedan sastojak.\nTrenutno je baza sa sastojima prazna.\nIdite na karticu sastojci", "Napomena");
            }
        }

        private void LoginRegisterExecute()
        {
            try
            {
                LoginService s = new LoginService();
                // Hash Password
                var hasher = new SHA256Managed();
                var unhashed = Encoding.Unicode.GetBytes(login.passwordBox.Password.ToString());
                var hashed = hasher.ComputeHash(unhashed);
                var hashedPassword = Convert.ToBase64String(hashed);
                this.User.Username = login.NameTextBox.Text.ToString();
                this.User.UserPassword = hashedPassword;
                this.User.FirstLastName = login.nameSurnameUser.Text.ToString();

                if (s.AddUser(User) != null)
                {
                    IsUpdateUser = true;
                    usersLogin = true;
                    LoggedGuest.NameSurname = login.nameSurnameUser.Text.ToString();
                    LoggedGuest.Username = login.NameTextBox.Text.ToString();
                    LoggedGuest.ID = User.UserID;
                    login.pnlRegistrationUser.Visibility = Visibility.Collapsed;
                    login.pnlSuccessfulRegistration.Visibility = Visibility.Visible;
                    OpenMainMenu();
                    MessageIngredient();
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString());
            }
        }

        private bool CanLoginRegisterEExecute()
        {
            return true;
        }

        /// <summary>
        /// Login Guest
        /// </summary>
        private ICommand loginUser;
        public ICommand LoginUser
        {
            get
            {
                if (loginUser == null)
                {
                    loginUser = new RelayCommand(param => LoginUserExecute(), param => CanLoginUserExecute());
                }
                return loginUser;
            }
        }

        public async void OpenMainMenu()
        {
            await Task.Delay(2000);
            login.Close();
        }

        private void LoginUserExecute()
        {
            try
            {
                LoginService s = new LoginService();

                string username = login.NameTextBox.Text;

                //uniqueness check username
                tblUser usertUsername = s.GetUserUsername(username);

                // Hash password
                var hasher = new SHA256Managed();
                var unhashed = Encoding.Unicode.GetBytes(login.passwordBox.Password);
                var hashed = hasher.ComputeHash(unhashed);
                var hashedPassword = Convert.ToBase64String(hashed);

                string password = hashedPassword;

                //Checks if there is a username and password in the database
                tblUser userLogin = s.GetUsernamePassword(username, password);

                if (login.NameTextBox.Text == "Admin" && login.passwordBox.Password == "Admin123")
                {
                    login.pnlLoginUser.Visibility = Visibility.Collapsed;
                    login.pnlSuccessfulLogin.Visibility = Visibility.Visible;
                    LoggedGuest.NameSurname = "Administrator";
                    LoggedGuest.Username = login.NameTextBox.ToString();
                    LoggedGuest.ID = 0;
                    adminLogin = true;
                    OpenMainMenu();
                    MessageIngredient();
                }
                else if (login.NameTextBox.Text.ToLower() == "admin")
                {
                    login.SnackError();
                    return;
                }
                else if (userLogin != null)
                {
                    LoggedGuest.NameSurname = userLogin.FirstLastName;
                    LoggedGuest.Username = userLogin.Username;
                    LoggedGuest.ID = userLogin.UserID;
                    usersLogin = true;
                    login.pnlLoginUser.Visibility = Visibility.Collapsed;
                    login.pnlSuccessfulLogin.Visibility = Visibility.Visible;
                    OpenMainMenu();
                    MessageIngredient();
                }
                else if (usertUsername != null)
                {
                    login.SnackError();
                    return;
                }
                else
                {
                    login.pnlLoginUser.Visibility = Visibility.Collapsed;
                    login.pnlRegistrationUser.Visibility = Visibility.Visible;
                    login.nameSurnameUser.Focus();
                }
            }
            catch (Exception)
            {

            }
        }

        private bool CanLoginUserExecute()
        {
            return true;
        }

        public async void VisibleLoginFail()
        {
            login.loginFail.Visibility = Visibility.Visible;
            await Task.Delay(2000);
            login.loginFail.Visibility = Visibility.Collapsed;
        }

        private ICommand backToLogin;
        public ICommand BackToLogin
        {
            get
            {
                if (backToLogin == null)
                {
                    backToLogin = new RelayCommand(param => BackLoginExecute(), param => CanBackLoginExecute());
                }
                return backToLogin;
            }
        }

        //Return to the login page
        private void BackLoginExecute()
        {
            login.pnlRegistrationUser.Visibility = Visibility.Collapsed;
            login.pnlLoginUser.Visibility = Visibility.Visible;
            login.NameTextBox.Text = null;
            login.passwordBox.Password = null;
            login.nameSurnameUser.Text = null;

            return;
        }
        private bool CanBackLoginExecute()
        {
            return true;
        }
        #endregion
    }
}