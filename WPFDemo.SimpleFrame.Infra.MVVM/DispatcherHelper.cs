using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.MVVM
{
    public class DispatcherHelper
    {
        public static void InvokeOnUIThread(Action action)
        {
            if(action == null)
            {
                throw new ArgumentNullException("action is null");
            }

            if(Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() => WrapAction(action)));
            }
        }

        private static void WrapAction(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "");
            }
        }
    }
}
