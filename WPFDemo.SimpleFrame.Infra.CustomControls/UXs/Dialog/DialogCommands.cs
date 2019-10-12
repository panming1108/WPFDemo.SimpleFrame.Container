using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.Dialog
{
    public static class DialogCommands
    {
        private enum CommandId
        {
            Confirm,
            Cancel,
            Close
        }

        private static readonly int CommandsCount = Enum.GetNames(typeof(CommandId)).Length;

        private static readonly RoutedUICommand[] InternalCommands = new RoutedUICommand[CommandsCount];

        private static string GetUICommandText(CommandId commandId)
        {
            switch (commandId)
            {
                case CommandId.Confirm:
                    return "确定";
                case CommandId.Cancel:
                    return "取消";
                case CommandId.Close:
                    return "关闭";
                default:
                    return string.Empty;
            }
        }

        public static RoutedUICommand ConfirmCommand
        {
            get
            {
                return EnsureCommand(CommandId.Confirm);
            }
        }

        public static RoutedUICommand CancelCommand
        {
            get
            {
                return EnsureCommand(CommandId.Cancel);
            }
        }

        public static RoutedUICommand CloseCommand
        {
            get
            {
                return EnsureCommand(CommandId.Close);
            }
        }

        private static RoutedUICommand EnsureCommand(CommandId commandId)
        {
            lock(InternalCommands.SyncRoot)
            {
                if(InternalCommands[(int)commandId] == null)
                {
                    RoutedUICommand routedUICommand = new RoutedUICommand(GetUICommandText(commandId), commandId.ToString(), typeof(DialogCommands));
                    InternalCommands[(int)commandId] = routedUICommand;
                }
            }
            return InternalCommands[(int)commandId];
        }
    }
}
