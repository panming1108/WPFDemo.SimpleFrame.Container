using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.UXs.Dialog
{
    public class DialogBase : Window
    {
        protected Action _action;
        public DialogBase()
        {
            this.Style = FindResource("BaseDialogStyle") as Style;
            Rect rc = SystemParameters.WorkArea;//获取工作区大小
            this.Left = 0;//设置位置
            this.Top = 0;
            this.Width = rc.Width;
            this.Height = rc.Height;
        }

        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsClosed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsClosedProperty =
            DependencyProperty.Register("IsClosed", typeof(bool), typeof(DialogBase), new PropertyMetadata(IsClosedChanged));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(DialogBase), new PropertyMetadata(string.Empty));


        public Window WindowOwner { get => base.Owner; set => base.Owner = value; }


        private static void IsClosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (d != null)
                {
                    DialogBase baseWinDialog = d as DialogBase;

                    if ((bool)e.NewValue)
                    {
                        baseWinDialog.Close();
                    }
                }
            }));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Owner?.Activate();
        }
    }
}
