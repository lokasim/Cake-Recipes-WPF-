using CakeRecipes.Models;
using CakeRecipes.Services;
using CakeRecipes.Views;
using System.Collections.Generic;
using System.Linq;

namespace CakeRecipes.ViewModel
{
    class AllStorageViewModel : ViewModelBase
    {
        StorageService storagedata = new StorageService();
        readonly AllStorageList allStorageListWindow;

        #region Constructor
        /// <summary>
        /// Constructor with the AllStorageList info window opening
        /// </summary>
        /// <param name="allStorageWindowOpen">opends the window</param>
        public AllStorageViewModel(AllStorageList allStorageWindowOpen)
        {
            allStorageListWindow = allStorageWindowOpen;
            StorageList = storagedata.GetAllSelectedIngredientStorageItems(LoggedGuest.ID).ToList();
        }
        #endregion

        #region Property
        /// <summary>
        /// List of storage items
        /// </summary>
        private List<tblIngredientStorage> storageList;
        public List<tblIngredientStorage> StorageList
        {
            get
            {
                return storageList;
            }
            set
            {
                storageList = value;
                OnPropertyChanged("StorageList");
            }
        }

        /// <summary>
        /// Specific storage item
        /// </summary>
        private tblShoppingBasket storageItem;
        public tblShoppingBasket StorageItem
        {
            get
            {
                return storageItem;
            }
            set
            {
                storageItem = value;
                OnPropertyChanged("StorageItem");
            }
        }
        #endregion
    }
}
