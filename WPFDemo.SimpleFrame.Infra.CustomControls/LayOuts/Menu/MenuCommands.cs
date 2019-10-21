using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.LayOuts.Menu
{
    public class MenuCommands
    {
        /// </summary>
        public static ICommand SelectedCommand
        {
            get;
            set;
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.CanExecuteCommand != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this.CanExecuteCommand != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public Action<T> ExecuteCommand { get; private set; }
        public Predicate<T> CanExecuteCommand { get; private set; }

        public DelegateCommand(Action<T> executedCommand, Predicate<T> canExecutedCommand)
        {
            this.ExecuteCommand = executedCommand;
            this.CanExecuteCommand = canExecutedCommand;
        }
        public DelegateCommand(Action<T> executedCommand) : this(executedCommand, null)
        {

        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand == null || CanExecuteCommand((T)parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteCommand?.Invoke((T)parameter);
        }
    }
}
