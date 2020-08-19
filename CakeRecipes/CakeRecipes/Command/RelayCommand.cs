using System;
using System.Windows.Input;

namespace CakeRecipes.Command
{
    class RelayCommand : ICommand
    {
        #region Fields readonly
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion

        #region Constuctors
        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }
        #endregion
        #region ICommand Members [DebuggerStepThrough]
        public bool CanExecute(object parameter) { return _canExecute == null ? true : _canExecute(parameter); }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter) { _execute(parameter); }
        #endregion
    }
}
