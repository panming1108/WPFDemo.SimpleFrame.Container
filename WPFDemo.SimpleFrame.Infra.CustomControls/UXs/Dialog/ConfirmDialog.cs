using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.Dialog
{
    public class ConfirmDialog : DialogBase, IConfirmDialog
    {
        private CommandBinding ConfirmCommandBinding;

        public ConfirmDialog()
        {
            RegisterCommand(this, ConfirmCommandBinding, DialogCommands.ConfirmCommand, OnConfirmExecuted, CanConfirmExecuted);
            this.Style = FindResource("ConfirmDialogStyle") as Style;
        }

        public bool? ShowDialog(string title, string content, Action action)
        {
            Title = title;
            Text = content;
            _action = action;
            return ShowDialog();
        }

        private void CanConfirmExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            ConfirmDialog confirmDialog = sender as ConfirmDialog;
            if(confirmDialog != null)
            {
                e.CanExecute = true;
            }
        }

        private void OnConfirmExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _action?.Invoke();
            DialogResult = true;
            this.Close();
        }

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
