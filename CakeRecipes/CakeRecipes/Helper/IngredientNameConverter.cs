using System;
using System.Windows.Data;
using System.Globalization;
using CakeRecipes.Services;

namespace CakeRecipes.Helper
{
    /// <summary>
    ///  Gets the name of the ingredient for the given id
    /// </summary>
    class IngredientNameConverter : IValueConverter
    {
        /// <summary>
        /// Convers the ingredient id to the name
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IngredientsData ingData = new IngredientsData();

            if (value != null)
            {
                for (int i = 0; i < ingData.GetAllIngredients().Count; i++)
                {
                    if (ingData.GetAllIngredients()[i].IngredientID == (int)value)
                    {
                        return ingData.GetAllIngredients()[i].IngredientName;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Returns the selected value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
