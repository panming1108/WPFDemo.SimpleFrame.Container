using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.ECGTools
{
    public static class BeatItemListViewCommands
    {
        private enum CommandId
        {
            SelectedAllCommand,
            ReverseSelectedCommand,
        }

        private static readonly int CommandCount = Enum.GetNames(typeof(CommandId)).Length;
        private static readonly RoutedUICommand[] InternalCommands = new RoutedUICommand[CommandCount];

        public static ICommand SelectedAllCommand => EnsureCommand(CommandId.SelectedAllCommand);
        public static ICommand ReverseSelectedCommand => EnsureCommand(CommandId.ReverseSelectedCommand);

        private static ICommand EnsureCommand(CommandId commandId)
        {
            lock (InternalCommands.SyncRoot)
            {
                if(InternalCommands[(int)commandId] == null)
                {
                    RoutedUICommand routedUICommand = new RoutedUICommand(GetUIText(commandId), commandId.ToString(), typeof(BeatItemListViewCommands));
                    InternalCommands[(int)commandId] = routedUICommand;
                }
            }
            return InternalCommands[(int)commandId];
        }

        private static string GetUIText(CommandId commandId)
        {
            switch (commandId)
            {
                case CommandId.SelectedAllCommand:
                    return "全选";
                case CommandId.ReverseSelectedCommand:
                    return "反选";
                default:
                    return string.Empty;
            }
        }
    }
}
