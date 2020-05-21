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
    public class EMCWindow : System.Windows.Window ,IEMCWindow
    {
        private CommandBinding MinimizeCommandBinding;
        private CommandBinding MaximizeCommandBinding;
        private CommandBinding CloseCommandBinding;
        private CommandBinding RestoreCommandBinding;

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
            this.Style = (Style)this.FindResource(typeof(EMCWindow));
            this.MouseLeftButtonDown += EMCWindow_MouseLeftButtonDown;
            this.Unloaded += EMCWindow_Unloaded;
            MinimizeCommandBinding = new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeExecuted, CanMinimizeExecuted);
            MaximizeCommandBinding = new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeExecuted, CanResizeExecuted);
            RestoreCommandBinding = new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreExecuted, CanResizeExecuted);
            CloseCommandBinding = new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseExecuted, CanCloseExecuted);
            this.CommandBindings.Add(MinimizeCommandBinding);
            this.CommandBindings.Add(MaximizeCommandBinding);
            this.CommandBindings.Add(CloseCommandBinding);
            this.CommandBindings.Add(RestoreCommandBinding);
        }

        private void EMCWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            this.CommandBindings.Remove(MinimizeCommandBinding);
            this.CommandBindings.Remove(MaximizeCommandBinding);
            this.CommandBindings.Remove(CloseCommandBinding);
            this.CommandBindings.Remove(RestoreCommandBinding);
            this.MouseLeftButtonDown -= EMCWindow_MouseLeftButtonDown;
            this.Unloaded -= EMCWindow_Unloaded;
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
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }
        private void OnRestoreExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }


        private void OnMinimizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        #endregion

        #region CanExecuted
        private void CanCloseExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CanResizeExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void CanMinimizeExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }
        #endregion
    }
}
