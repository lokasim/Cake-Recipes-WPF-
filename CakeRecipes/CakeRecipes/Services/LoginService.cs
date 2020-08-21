using CakeRecipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CakeRecipes.Services
{
    class LoginService
    {
        public tblUser GetUsernamePassword(string username, string password)
        {
            try
            {
                using (CookDBEntities context = new CookDBEntities())
                {
                    tblUser user = (from e in context.tblUsers where e.Username.Equals(username) where e.UserPassword.Equals(password) select e).First();


                    return user;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        public tblUser AddUser(tblUser user)
        {
            try
            {

                using (CookDBEntities context = new CookDBEntities())
                {
                    if (user.UserID == 0)
                    {
                        tblUser newUser = new tblUser
                        {
                            UserID = user.UserID,
                            Username = user.Username,
                            UserPassword = user.UserPassword,
                            FirstLastName = user.FirstLastName
                        };

                        context.tblUsers.Add(newUser);
                        context.SaveChanges();
                        user.UserID = newUser.UserID;
                        return user;
                    }
                    else
                    {
                        tblUser userToEdit = (from r in context.tblUsers where r.UserID == user.UserID select r).First();

                        userToEdit.UserID = user.UserID;
                        userToEdit.Username = user.Username;
                        userToEdit.UserPassword = user.UserPassword;
                        userToEdit.FirstLastName = user.FirstLastName;
                        context.SaveChanges();
                        return user;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nešto je pošlo po zlu prilikom registracije", "Greška");
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message.ToString());
                return null;
            }
        }

        public tblUser GetUserUsername(string username)
        {
            try
            {
                using (CookDBEntities context = new CookDBEntities())
                {
                    tblUser user = (from e in context.tblUsers where e.Username == username select e).First();
                    
                    return user;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

    }
}
