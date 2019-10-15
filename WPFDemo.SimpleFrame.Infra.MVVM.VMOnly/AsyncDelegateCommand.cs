using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.MVVM.VMOnly
{
    public class AsyncDelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Func<Task> _executed;
        public Func<bool> _canExecuted;

        public AsyncDelegateCommand(Func<Task> executed, Func<bool> canExecuted)
        {
            _executed = executed;
            _canExecuted = canExecuted;
        }

        public AsyncDelegateCommand(Func<Task> executed) : this(executed, null)
        {
            
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuted == null || _canExecuted();
        }

        public async void Execute(object parameter)
        {
            await _executed?.Invoke();
        }
    }

    public class AsyncDelegateCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Func<T, Task> _executed;
        public Predicate<T> _canExecuted;

        public AsyncDelegateCommand(Func<T, Task> executed, Predicate<T> canExecuted)
        {
            _executed = executed;
            _canExecuted = canExecuted;
        }

        public AsyncDelegateCommand(Func<T, Task> executed) : this(executed, null)
        {

        }

        public bool CanExecute(object parameter)
        {
            return _canExecuted == null || _canExecuted((T)parameter);
        }

        public async void Execute(object parameter)
        {
            await _executed?.Invoke((T)parameter);
        }
    }
}
