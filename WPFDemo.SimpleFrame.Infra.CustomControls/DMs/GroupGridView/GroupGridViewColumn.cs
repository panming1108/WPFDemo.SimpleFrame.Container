using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public abstract class GroupGridViewColumn : DependencyObject
    {
        private bool _ignoreWidthChange;
        private GroupGridView _ownerGridView;
		protected internal GroupGridView OwnerGridView
		{
			get
			{
				return _ownerGridView;
			}
			internal set
			{
				_ownerGridView = value;
			}
		}

        public DataGridLength Width
        {
            get { return (DataGridLength)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(GroupGridViewColumn));       
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register(nameof(Width), typeof(DataGridLength), typeof(GroupGridViewColumn), new FrameworkPropertyMetadata(OnWidthPropertyChanged));

        private static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GroupGridViewColumn dataGridColumn = (GroupGridViewColumn)d;
            if(dataGridColumn.OwnerGridView == null)
            {
                return;
            }
            if(!dataGridColumn._ignoreWidthChange)
            {
                dataGridColumn._ignoreWidthChange = true;
                dataGridColumn.OwnerGridView.InternalColumns.SetAllColumnsWidth();
                dataGridColumn._ignoreWidthChange = false;
            }
        }

        internal void InitGridViewCell(GroupGridViewCell cell)
        {
            cell.Width = Width.DisplayValue;
            LoadedGridViewCell(cell);
        }
        protected abstract void LoadedGridViewCell(GroupGridViewCell cell);
    }
}
