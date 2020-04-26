using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDemo.SimpleFrame.Infra.Ioc;
using WPFDemo.SimpleFrame.IViewModelsEditors;

namespace WPFDemo.SimpleFrame.Views.Editors
{
    /// <summary>
    /// EMCInputDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class EMCInputDisplay : UserControl
    {
        public EMCInputDisplay()
        {
            InitializeComponent();
            DataContext = IocManagerInstance.ResolveType<IInputViewModel>();
        }

        private void EMCInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = 0.ToString();
            int number = int.Parse(textBox1.Text);
            if (number <= 100)
            {
                return;
            }
            textBox1.Text = "100";
            textBox1.SelectionStart = textBox1.Text.Length;
        }
    }
}
