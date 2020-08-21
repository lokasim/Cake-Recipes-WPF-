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

        private void LoginRegisterExecute()
        {          
            try
            {
                LoginService s = new LoginService();

                //string user = User.Username;
                

                ////uniqueness check username
                //tblPatient employeeUserPatient = s.GetPatientUsername(user);

                //if (employeeUserPatient != null)
                //{
                //    Xceed.Wpf.Toolkit.MessageBox.Show("Korisničko ime je zauzeto, pokušajte neko drugo.", "Korisničko ime");
                //    return;
                //}

                // Hash Password
                var hasher = new SHA256Managed();
                var unhashed = Encoding.Unicode.GetBytes(login.passwordBox.Password.ToString());
                var hashed = hasher.ComputeHash(unhashed);
                var hashedPassword = Convert.ToBase64String(hashed);
                this.User.Username = login.NameTextBox.Text.ToString();
                this.User.UserPassword = hashedPassword;
                this.User.FirstLastName = login.nameSurnameUser.Text.ToString();

                

                s.AddUser(User);
                IsUpdateUser = true;
                usersLogin = true;
                LoggedGuest.NameSurname = login.nameSurnameUser.Text.ToString();
                LoggedGuest.Username = login.NameTextBox.Text.ToString();
                LoggedGuest.ID = User.UserID;
                login.pnlRegistrationUser.Visibility = Visibility.Collapsed;
                login.pnlSuccessfulRegistration.Visibility = Visibility.Visible;

                OpenMainMenu();
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
                }
                else if (login.NameTextBox.Text == "Admin" || login.NameTextBox.Text == "admin")
                {
                    login.SnackError();
                    //Xceed.Wpf.Toolkit.MessageBox.Show("Korisničko ime je rezervisano, pokušajte sa nekim drugim.", "Korisničko ime");
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
                    //Xceed.Wpf.Toolkit.MessageBox.Show($"{LoggedGuest.NameSurname}, dobrodošli.", "Recepti");
                }
                else if (usertUsername != null)
                {
                    login.SnackError();
                    //Xceed.Wpf.Toolkit.MessageBox.Show("Korisničko ime je zauzeto, pokušajte neko drugo.", "Korisničko ime");
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
            //if (login.JMBG.Text.Length != 13)
            //{
            //    login.jmbgHint.Foreground = new SolidColorBrush(Colors.Red);
            //    return false;
            //}
            //else
            //{
            //    login.jmbgHint.Foreground = new SolidColorBrush(Colors.Green);
            return true;
            //}
        }

        public async void VisibleLoginFail()
        {
            login.loginFail.Visibility = Visibility.Visible;
            await Task.Delay(2000);
            login.loginFail.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// View guest login form
        /// </summary>
        private ICommand loginGuest;
        public ICommand LoginGuest
        {
            get
            {
                if (loginGuest == null)
                {
                    loginGuest = new RelayCommand(param => LoginGuestExecute(), param => CanLoginGuestExecute());
                }
                return loginGuest;
            }
        }

        private void LoginGuestExecute()
        {
            //login.pnlLoginEmployee.Visibility = Visibility.Collapsed;
            //login.pnlLoginGuest.Visibility = Visibility.Visible;
            //login.pnlSignUpGuest.Visibility = Visibility.Collapsed;
            //login.loginFail.Visibility = Visibility.Collapsed;
        }

        private bool CanLoginGuestExecute()
        {
            //if (login.pnlLoginGuest.Visibility == Visibility.Visible)
            //{
            //    return false;
            //}
            //else
            //{
                return true;
            //}
        }

        /// <summary>
        /// View Guest login form
        /// </summary>
        private ICommand loginEmployee;
        public ICommand LoginEmployee
        {
            get
            {
                if (loginEmployee == null)
                {
                    loginEmployee = new RelayCommand(param => LoginEmployeeExecute(), param => CanLoginEmployeeExecute());
                }
                return loginEmployee;
            }
        }

        private void LoginEmployeeExecute()
        {
            //login.pnlLoginGuest.Visibility = Visibility.Collapsed;
            //login.pnlLoginEmployee.Visibility = Visibility.Visible;
            //login.pnlSignUpGuest.Visibility = Visibility.Collapsed;
            //login.loginFail.Visibility = Visibility.Collapsed;
        }

        private bool CanLoginEmployeeExecute()
        {
            //if (login.pnlLoginEmployee.Visibility == Visibility.Visible)
            //{
            //    return false;
            //}
            //else
            //{
                return true;
           // }
        }

        /// <summary>
        /// View guest registration form
        /// </summary>
        private ICommand signUpGuest;
        public ICommand SignUpGuest
        {
            get
            {
                if (signUpGuest == null)
                {
                    signUpGuest = new RelayCommand(param => SignUpGuestExecute(), param => CanSignUpGuestExecute());
                }
                return signUpGuest;
            }
        }

        private void SignUpGuestExecute()
        {
            //login.pnlLoginGuest.Visibility = Visibility.Collapsed;
            //login.pnlLoginEmployee.Visibility = Visibility.Collapsed;
            //login.pnlSignUpGuest.Visibility = Visibility.Visible;
            //login.error.Visibility = Visibility.Collapsed;
            //login.txtName.Text = "";
            //login.txtSurname.Text = "";
            //login.txtJMBG.Text = "";
            //login.txtEmail.Text = "";
            //login.JMBG.Text = "";
            //login.passwordBox.Password = "";
            //login.loginFail.Visibility = Visibility.Collapsed;
        }

        private bool CanSignUpGuestExecute()
        {
            return true;
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

        private ICommand signUp;
        public ICommand SignUp
        {
            get
            {
                if (signUp == null)
                {
                    signUp = new RelayCommand(param => SignUpExecute(), param => CanSignUpExecute());
                }
                return signUp;
            }
        }

        /// <summary>
        /// A method for registering new guests
        /// </summary>
        private void SignUpExecute()
        {
            //try
            //{
            //    Service s = new Service();

            //    string name = User.GuestName;
            //    string surname = User.GuestSurname;
            //    string jmbg = User.JMBG;
            //    string email = User.EMail;

            //    //JMBG validation
            //    if (!ValidationJMBG.CheckJMBG(jmbg))
            //    {
            //        return;
            //    }

            //    //Check if the JMBG exists in the database
            //    tblGuest employee = s.GetGuestJMBG(jmbg);

            //    if (employee != null)
            //    {
            //        Xceed.Wpf.Toolkit.MessageBox.Show("JMBG already exists in the database, try another.", "JMBG");
            //        return;
            //    }

            //    //Check if the email exists in the database
            //    tblGuest employeeEmail = s.GetGuestEmail(email);

            //    if (employeeEmail != null)
            //    {
            //        Xceed.Wpf.Toolkit.MessageBox.Show("E-mail already exists in the database, try another.", "E-mail");
            //        return;
            //    }

            //    s.AddGuest(User);

            //    IsUpdateUser = true;

            //    string poruka = "Guest: " + User.GuestName + " " + User.GuestSurname;
            //    Xceed.Wpf.Toolkit.MessageBox.Show(poruka, "Successfully added Guest", MessageBoxButton.OK);
            //    login.txtName.Text = "";
            //    login.txtSurname.Text = "";
            //    login.txtJMBG.Text = "";
            //    login.txtEmail.Text = "";
            //    login.pnlLoginGuest.Visibility = Visibility.Visible;
            //    login.pnlLoginEmployee.Visibility = Visibility.Collapsed;
            //    login.pnlSignUpGuest.Visibility = Visibility.Collapsed;
            //}
            //catch (Exception ex)
            //{
            //    Xceed.Wpf.Toolkit.MessageBox.Show(ex.ToString());
            //}
        }

        private bool CanSignUpExecute()
        {
            //if (String.IsNullOrEmpty(user.GuestName) || String.IsNullOrWhiteSpace(user.GuestName) ||
            //    String.IsNullOrEmpty(user.GuestSurname) || String.IsNullOrWhiteSpace(user.GuestSurname) ||
            //    String.IsNullOrEmpty(user.JMBG) ||
            //    String.IsNullOrEmpty(user.EMail))
            //{
            //    return false;
            //}
            //else if (user.JMBG.Length != 13)
            //{
            //    return false;
            //}
            //else if (!Regex.IsMatch(login.txtEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            //{
            //    return false;
            //}
            //else
            //{
            return true;
            //}
        }
        #endregion
    }
}