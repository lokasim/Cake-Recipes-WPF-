using CakeRecipes.Command;
using CakeRecipes.Models;
using CakeRecipes.Services;
using CakeRecipes.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CakeRecipes.ViewModel
{
    class AllShoppingListViewModel : ViewModelBase
    {
        #region Variables
        ShoppingService shoppingData = new ShoppingService();
        IngredientService ingredientData = new IngredientService();
        StorageService storagedata = new StorageService();
        readonly AllShoppingList allShoppingListWindow;
        public static bool isShoppingListNotUpdated = false;
        /// <summary>
        /// Background worker
        /// </summary>
        private readonly BackgroundWorker bgWorker = new BackgroundWorker();
        /// <summary>
        /// Check if background worker is running
        /// </summary>
        private bool _isRunning = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with the AllShoppingList info window opening
        /// </summary>
        /// <param name="allShoppingWindowOpen">opends the window</param>
        public AllShoppingListViewModel(AllShoppingList allShoppingWindowOpen)
        {
            allShoppingListWindow = allShoppingWindowOpen;
            bgWorker.DoWork += WorkerOnDoWork;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.ProgressChanged += WorkerOnProgressChanged;
            bgWorker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
            ProgressBarVisibility = Visibility.Collapsed;
            ButtonVisibility = Visibility.Visible;
            ShoppingBasketList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
            StorageList = storagedata.GetAllSelectedIngredientStorageItems(LoggedGuest.ID).ToList();
            storagedata.OnNotification += storagedata.AddIngredientStorage;
        }
        #endregion

        #region Property
        /// <summary>
        /// List of recipes
        /// </summary>
        private List<tblShoppingBasket> shoppingBasketList;
        public List<tblShoppingBasket> ShoppingBasketList
        {
            get
            {
                return shoppingBasketList;
            }
            set
            {
                shoppingBasketList = value;
                OnPropertyChanged("ShoppingBasketList");
            }
        }

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
        /// Specific shopping Basket
        /// </summary>
        private tblShoppingBasket shoppingBasket;
        public tblShoppingBasket ShoppingBasket
        {
            get
            {
                return shoppingBasket;
            }
            set
            {
                shoppingBasket = value;
                OnPropertyChanged("ShoppingBasket");
            }
        }

        /// <summary>
        /// Info label
        /// </summary>
        private string infoLabel;
        public string InfoLabel
        {
            get
            {
                return infoLabel;
            }
            set
            {
                infoLabel = value;
                OnPropertyChanged("InfoLabel");
            }
        }

        /// <summary>
        /// Info label background
        /// </summary>
        private string infoLabelBG;
        public string InfoLabelBG
        {
            get
            {
                return infoLabelBG;
            }
            set
            {
                infoLabelBG = value;
                OnPropertyChanged("InfoLabelBG");
            }
        }

        /// <summary>
        /// ProgressBar Info Label
        /// </summary>
        private string progressBarInfoLabel;
        public string ProgressBarInfoLabel
        {
            get
            {
                return progressBarInfoLabel;
            }
            set
            {
                progressBarInfoLabel = value;
                OnPropertyChanged("ProgressBarInfoLabel");
            }
        }

        /// <summary>
        /// The progress bar property
        /// </summary>
        private int currentProgress;
        public int CurrentProgress
        {
            get
            {
                return currentProgress;
            }
            set
            {
                if (currentProgress != value)
                {
                    currentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }

        /// <summary>
        /// The progress bar property
        /// </summary>
        private Visibility progressBarVisibility;
        public Visibility ProgressBarVisibility
        {
            get
            {
                return progressBarVisibility;
            }
            set
            {
                progressBarVisibility = value;
                OnPropertyChanged("ProgressBarVisibility");
            }
        }

        /// <summary>
        /// The button property
        /// </summary>
        private Visibility buttonVisibility;
        public Visibility ButtonVisibility
        {
            get
            {
                return buttonVisibility;
            }
            set
            {
                buttonVisibility = value;
                OnPropertyChanged("ButtonVisibility");
            }
        }
        #endregion

        #region Background worker
        /// <summary>
        /// Updates the progress bar and prints the value
        /// </summary>
        /// <param name="sender">objecy sender</param>
        /// <param name="e">progress changed event</param>
        private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
            ProgressBarInfoLabel = CurrentProgress + " %";
        }

        /// <summary>
        /// Method that the background worker executes
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">do work event</param>
        private void WorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            ProgressBarVisibility = Visibility.Visible;
            ButtonVisibility  = Visibility.Collapsed;
            Random rng = new Random();

            for (int i = 1; i < 22; i++)
            {
                Thread.Sleep(100);

                if (i == 21)
                {
                    // 100% if all passed 
                    // Notify about an item being purchased
                    storagedata.Notify(ShoppingBasket);
                    StorageList = storagedata.GetAllSelectedIngredientStorageItems(LoggedGuest.ID).ToList();

                    // Write to file the changes
                    ReadWriteToFile rwf = new ReadWriteToFile();
                    rwf.WriteShoppingFile(ShoppingBasket);

                    // Delete item from the shopping basket after 2 seconds
                    shoppingData.DeleteShoppingBasket(ShoppingBasket.ShoppingBasketID);
                    ShoppingBasketList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
                    // allShoppingListWindow.DataGridShoppingList.UnselectAll();

                    bgWorker.ReportProgress(100);
                }

                bgWorker.ReportProgress(100 / 20 * i);
            }
            
            _isRunning = false;

            // Cancel the asynchronous operation if still in progress
            if (bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
            }
        }

        /// <summary>
        /// Print the appropriate message depending how the worker finished.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">worker completed event</param>
        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                progressBarInfoLabel = e.Error.Message;
                _isRunning = false;
            }
            else
            {
                tblIngredient ing = ingredientData.FindIngredient(ShoppingBasket.IngredientID);
                InfoLabel = "Uspesno izvrsena porudzbina sastojka " + ing.IngredientName;
                InfoLabelBG = "#28a745";
                ProgressBarVisibility = Visibility.Collapsed;
                ButtonVisibility = Visibility.Visible;
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Delete Shopping Basket button
        /// </summary>
        private ICommand deleteShoppingBasket;
        public ICommand DeleteShoppingBasket
        {
            get
            {
                if (deleteShoppingBasket == null)
                {
                    deleteShoppingBasket = new RelayCommand(param => DeleteShoppingBasketExecute(), param => CanDeleteShoppingBasketExecute());
                }
                return deleteShoppingBasket;
            }
        }

        /// <summary>
        /// Method for deleting the selected item from the list
        /// </summary>
        public void DeleteShoppingBasketExecute()
        {
            try
            {
                MessageBoxResult dialogDelete = Xceed.Wpf.Toolkit.MessageBox.Show($"Da li zelite da obrisete ovaj sastojak iz liste?", "Obrisi recept", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (dialogDelete == MessageBoxResult.Yes)
                {
                    if (ShoppingBasket != null)
                    {
                        shoppingData.DeleteShoppingBasket(ShoppingBasket.ShoppingBasketID);
                        ShoppingBasketList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
                    }
                }
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce obrisati sastojak...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if its possible to press the delete button
        /// </summary>
        /// <returns></returns>
        public bool CanDeleteShoppingBasketExecute()
        {
            if (ShoppingBasket == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Add Shopping Item button
        /// </summary>
        private ICommand addShoppingItem;
        public ICommand AddShoppingItem
        {
            get
            {
                if (addShoppingItem == null)
                {
                    addShoppingItem = new RelayCommand(param => AddShoppingItemExecute(), param => CanAddShoppingItemExecute());
                }
                return addShoppingItem;
            }
        }

        /// <summary>
        /// Method for adding the selected item from the list
        /// </summary>
        public void AddShoppingItemExecute()
        {
            try
            {
                List<tblShoppingBasket> tempShoppingList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
                AddIngredientToRecipe addIngredientToBasket = new AddIngredientToRecipe();
                AddIngredientToRecipeWindow addIngredientToRecipeWindow = new AddIngredientToRecipeWindow();
                addIngredientToRecipeWindow.ShowDialog();
               // addIngredientToBasket.ShowDialog();

                // Return the list to the initial state
                if (isShoppingListNotUpdated == true)
                {
                    for (int i = 0; i < tempShoppingList.Count; i++)
                    {
                        tempShoppingList[i].ShoppingBasketID = 0;
                        tempShoppingList[i].UserID = LoggedGuest.ID;
                        shoppingData.AddShoppingList(tempShoppingList[i]);
                    }
                    isShoppingListNotUpdated = false;
                }

                ShoppingBasketList = shoppingData.GetAllSelectedShoppingBasketItems(LoggedGuest.ID).ToList();
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce dodati sastojak...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if its possible to press the add button
        /// </summary>
        /// <returns></returns>
        public bool CanAddShoppingItemExecute()
        {
            if (ingredientData.GetAllIngredients().Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Purchase Item button
        /// </summary>
        private ICommand purchaseItem;
        public ICommand PurchaseItem
        {
            get
            {
                if (purchaseItem == null)
                {
                    purchaseItem = new RelayCommand(param => PurchaseItemExecute(), param => CanPurchaseItemExecute());
                }
                return purchaseItem;
            }
        }

        /// <summary>
        /// Method for purchasing selected item from the list
        /// </summary>
        public void PurchaseItemExecute()
        {
            try
            {
                if (!bgWorker.IsBusy && ShoppingBasket != null)
                {
                    InfoLabelBG = "#17a2b8";
                    InfoLabel = "Kupovina u toku...";
                    // This method will start the execution asynchronously in the background
                    bgWorker.RunWorkerAsync();
                    _isRunning = true;
                }
                else if (bgWorker.IsBusy)
                {
                    InfoLabelBG = "#ffc107";
                    InfoLabel = "Proces kupovine je u toku, molim Vas sacekajte.";
                }
            }
            catch (Exception)
            {
                MessageBoxResult dialog = Xceed.Wpf.Toolkit.MessageBox.Show("Trenutno je nemoguce dodati sastojak...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if its possible to press the add button
        /// </summary>
        /// <returns></returns>
        public bool CanPurchaseItemExecute()
        {
            if (ShoppingBasket == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
