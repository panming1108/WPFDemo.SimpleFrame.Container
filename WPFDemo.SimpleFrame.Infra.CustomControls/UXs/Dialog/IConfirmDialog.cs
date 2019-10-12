using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.Dialog
{
    public interface IConfirmDialog
    {
        bool? ShowDialog(string title, string content, Action action);
    }
}
