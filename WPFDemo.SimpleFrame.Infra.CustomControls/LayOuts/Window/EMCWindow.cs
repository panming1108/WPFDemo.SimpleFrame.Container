using Microsoft.Windows.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.LayOuts.Window
{
    public class EMCWindow : System.Windows.Window
    {
        private CommandBinding MinimizeCommandBinding;
        private CommandBinding MaximizeCommandBinding;
        private CommandBinding CloseCommandBinding;

        public Brush HeadBackground
        {
            get { return (Brush)GetValue(HeadBackgroundProperty); }
            set { SetValue(HeadBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeadBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadBackgroundProperty =
            DependencyProperty.Register("HeadBackground", typeof(Brush), typeof(EMCWindow));

        public EMCWindow()
        {
            var style = this.FindResource("EMCWindowStyle");
            if(style != null)
            {
                this.Style = style as Style;
            }
            this.MouseLeftButtonDown += EMCWindow_MouseLeftButtonDown;
            RegisterCommand(this, MinimizeCommandBinding, SystemCommands.MinimizeWindowCommand, OnMinimizeExecuted, CanMinimizeExecuted);
            RegisterCommand(this, MaximizeCommandBinding, SystemCommands.MaximizeWindowCommand, OnMaximizeExecuted, CanMaximizeExecuted);
            RegisterCommand(this, CloseCommandBinding, SystemCommands.CloseWindowCommand, OnCloseExecuted, CanCloseExecuted);
        }

        private void EMCWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #region Executed
        private void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void OnMaximizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void OnMinimizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        #region CanExecuted
        private void CanCloseExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CanMaximizeExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CanMinimizeExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        private void RegisterCommand(UIElement ui, CommandBinding bind, ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            //CommandManager.RegisterClassCommandBinding(typeof(EMCDataPager), new CommandBinding(command, executed, canExecute));
            if (bind != null)
            {
                bind.Executed -= executed;
                bind.CanExecute -= canExecute;
                bind = null;
            }
            bind = new CommandBinding(command);
            bind.Executed += executed;
            bind.CanExecute += canExecute;
            ui.CommandBindings.Add(bind);
        }

        private void UnRegisterCommand(UIElement ui, CommandBinding bind, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
        {
            //CommandManager.RegisterClassCommandBinding(typeof(EMCDataPager), new CommandBinding(command, executed, canExecute));
            if (bind != null)
            {
                bind.Executed -= executed;
                bind.CanExecute -= canExecute;
                ui.CommandBindings.Remove(bind);
                bind = null;
            }
        }
    }
}
