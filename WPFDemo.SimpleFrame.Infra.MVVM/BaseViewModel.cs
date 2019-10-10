using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.MVVM
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
            _loadedCommand = new DelegateCommand(Loaded);
            _unLoadedCommand = new DelegateCommand(UnLoaded);
        }

        protected abstract void UnLoaded();

        protected abstract void Loaded();

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
