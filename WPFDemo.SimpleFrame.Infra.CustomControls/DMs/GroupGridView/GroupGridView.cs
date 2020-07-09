using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    public class GroupGridView : ItemsControl
    {
		private readonly GroupGridViewColumnCollection _columns;
        public ObservableCollection<GroupGridViewColumn> Columns => _columns;
		internal GroupGridViewColumnCollection InternalColumns => _columns;

		private readonly GroupGridViewRowCollection _rows;
		internal GroupGridViewRowCollection Rows => _rows;

        public double ColumnHeaderHeight
		{
            get { return (double)GetValue(ColumnHeaderHeightProperty); }
            set { SetValue(ColumnHeaderHeightProperty, value); }
        }

        public double RowHeight
        {
            get { return (double)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        public string ItemsSourceDisplayMemberPath
        {
            get { return (string)GetValue(ItemsSourceDisplayMemberPathProperty); }
            set { SetValue(ItemsSourceDisplayMemberPathProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceDisplayMemberPathProperty =
            DependencyProperty.Register(nameof(ItemsSourceDisplayMemberPath), typeof(string), typeof(GroupGridView));

        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register(nameof(RowHeight), typeof(double), typeof(GroupGridView));

        public static readonly DependencyProperty ColumnHeaderHeightProperty =
            DependencyProperty.Register(nameof(ColumnHeaderHeight), typeof(double), typeof(GroupGridView));

        public GroupGridView()
        {
            _columns = new GroupGridViewColumnCollection(this);
			_rows = new GroupGridViewRowCollection(this);
		}

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
			_columns.SetAllColumnsWidth();
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new GroupGridViewRow();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is GroupGridViewRow;
		}

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
			GroupGridViewRow row = element as GroupGridViewRow;
			row.ParentGridView = this;
			row.ParentItemControl = this;
			row.ItemsSourceDisplayMemberPath = ItemsSourceDisplayMemberPath;
			if (!string.IsNullOrEmpty(ItemsSourceDisplayMemberPath))
            {
				row.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(ItemsSourceDisplayMemberPath));
            }
			Rows.Add(row);
		}
    }
}
