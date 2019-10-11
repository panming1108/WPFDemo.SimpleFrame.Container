using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataPager
{
    public static class DataPagerCommands
    {
        private enum CommandId
        {
            /// <summary>
            /// 跳转到第一页 command.
            /// </summary>
            MoveToFirstPage,
            /// <summary>
            /// 跳转到最后一页 command.
            /// </summary>
            MoveToLastPage,
            /// <summary>
            /// 跳转到下一页 command.
            /// </summary>
            MoveToNextPage,
            /// <summary>
            /// 跳转至 command.
            /// </summary>
            MoveToPage,
            /// <summary>
            /// 跳转到前一页 command.
            /// </summary>
            MoveToPreviousPage
        }

        private static readonly int CommandsCount = Enum.GetNames(typeof(CommandId)).Length;

        private static readonly RoutedUICommand[] InternalCommands = new RoutedUICommand[CommandsCount];

        /// <summary>
		/// 转至首页
		/// </summary>
		public static ICommand MoveToFirstPage
        {
            get
            {
                return EnsureCommand(CommandId.MoveToFirstPage);
            }
        }
        /// <summary>
        /// 转至末页
        /// </summary>
        public static ICommand MoveToLastPage
        {
            get
            {
                return EnsureCommand(CommandId.MoveToLastPage);
            }
        }
        /// <summary>
        /// 向后一页
        /// </summary>
        public static ICommand MoveToNextPage
        {
            get
            {
                return EnsureCommand(CommandId.MoveToNextPage);
            }
        }
        /// <summary>
        /// 转至
        /// </summary>
        public static ICommand MoveToPage
        {
            get
            {
                return EnsureCommand(CommandId.MoveToPage);
            }
        }
        /// <summary>
        /// 向前一页
        /// </summary>
        public static ICommand MoveToPreviousPage
        {
            get
            {
                return EnsureCommand(CommandId.MoveToPreviousPage);
            }
        }
        private static RoutedUICommand EnsureCommand(CommandId commandId)
        {
            lock (InternalCommands.SyncRoot)
            {
                if (InternalCommands[(int)commandId] == null)
                {
                    RoutedUICommand routedUICommand = new RoutedUICommand(GetUIText(commandId), commandId.ToString(), typeof(DataPagerCommands));
                    InternalCommands[(int)commandId] = routedUICommand;
                }
            }
            return InternalCommands[(int)commandId];
        }
        private static string GetUIText(CommandId commandId)
        {
            switch (commandId)
            {
                case CommandId.MoveToFirstPage:
                    return "转至首页";
                case CommandId.MoveToLastPage:
                    return "转至末页";
                case CommandId.MoveToNextPage:
                    return "向后一页";
                case CommandId.MoveToPage:
                    return "转至";
                case CommandId.MoveToPreviousPage:
                    return "向前一页";
                default:
                    return string.Empty;
            }
        }
    }
}
