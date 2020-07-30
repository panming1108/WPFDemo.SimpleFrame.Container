using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Views.ECGTools
{
    public class MaskMenuHelper
    {
        public static ContextMenu GetContextMenu(IEnumerable menus)
        {
            ContextMenu contextMenu = new ContextMenu
            {
                ItemsSource = menus
            };
            return contextMenu;
        }
    }
}
