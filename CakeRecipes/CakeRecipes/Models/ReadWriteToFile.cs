using CakeRecipes.Services;
using CakeRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeRecipes.Models
{
    /// <summary>
    /// Reads or Writes from the file
    /// </summary>
    class ReadWriteToFile
    {
        IngredientService ingredientData = new IngredientService();
        private readonly object locker = new object();

        /// <summary>
        /// Writes to the user shopping basket file
        /// </summary>
        /// <param name="username">Super Admin username that is being added</param>
        /// <param name="password">Super Admin password</param>
        public void WriteShoppingFile(tblShoppingBasket item)
        {
            lock (locker)
            {
                string file = @"~\..\..\..\TextFiles\" + LoggedGuest.Username + ".txt";
                tblIngredient ing = ingredientData.FindIngredient(item.IngredientID);

                using (StreamWriter sw = new StreamWriter(file, append: true))
                {
                    sw.WriteLine("Kupljen je sastojak {0}. Kolicina {1}. Vreme {2}", ing.IngredientName, item.Amount, DateTime.Now.ToString("dd.MM.yyy HH:mm"));
                }
            }
        }
    }
}
