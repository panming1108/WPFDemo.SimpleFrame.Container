using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs.DataGrid
{
    public class CopyBtnAdorner : Adorner
    {
        private VisualCollection _visuals;
        private Canvas _grid;
        private Border _br;
        private Button button;

        public object Content { get; set; }
        public Style ButtonStyle { get; set; }

        public CopyBtnAdorner(UIElement adornedElement, Style buttonStyle) : base(adornedElement)
        {
            ButtonStyle = buttonStyle;
            _visuals = new VisualCollection(this);
            _br = new Border();
            button = new Button();
            if (ButtonStyle != null)
            {
                button.Style = ButtonStyle;
            }
            else
            {
                button.Width = 50;
                button.Height = 25;
                button.Content = "Copy";
            }
            button.Click += Button_Click;
            _br.Child = button;
            _grid = new Canvas();
            _grid.Children.Add(_br);
            //_grid.Visibility = Visibility.Hidden;
            _visuals.Add(_grid);

            
            //button = new Button();
            //if (ButtonStyle != null)
            //{
            //    button.Style = ButtonStyle;
            //}
            //else
            //{
            //    button.Width = 50;
            //    button.Height = 25;
            //    button.Content = "Copy";
            //}
            //button.Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(Content.ToString());
            Clipboard.SetText(Content.ToString());
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
            //return button;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            _grid.Arrange(new Rect(finalSize));
            _br.Margin = new Thickness(finalSize.Width - button.Width - 20, (finalSize.Height - button.Height) / 2, 0, 0);
            return base.ArrangeOverride(finalSize);

            //button.Arrange(new Rect(new Point(finalSize.Width - button.Width - 20, (finalSize.Height - button.Height) / 2), button.DesiredSize));
            //return base.ArrangeOverride(finalSize);
        }

        public void ShowButton()
        {
            _grid.Visibility = Visibility.Visible;
        }

        public void HideButton()
        {
            _grid.Visibility = Visibility.Hidden;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return _visuals.Count;
                //return 1;
            }
        }
    }
}
