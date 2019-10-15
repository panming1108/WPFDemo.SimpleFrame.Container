using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.MVVM.VMOnly
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected readonly ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand;

        protected readonly ICommand _unLoadedCommand;
        public ICommand UnLoadedCommand => _unLoadedCommand;

        public BaseViewModel()
        {
            _loadedCommand = new AsyncDelegateCommand(Loaded);
            _unLoadedCommand = new AsyncDelegateCommand(UnLoaded);
        }

        protected abstract Task UnLoaded();

        protected abstract Task Loaded();

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = (propertyExpression.Body as MemberExpression).Member.Name;
            OnPropertyChanged(propertyName);
        }
    }
}
