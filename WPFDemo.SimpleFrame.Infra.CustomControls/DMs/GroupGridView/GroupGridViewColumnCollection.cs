using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    internal class GroupGridViewColumnCollection : ObservableCollection<GroupGridViewColumn>
    {
        private readonly GroupGridView _ownerGridView;
        private double _allGridStarLength;
        private double _perStarLength;
        internal GroupGridViewColumnCollection(GroupGridView groupGridView)
        {
            _ownerGridView = groupGridView;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    UpdateGridViewReference(e.NewItems, clear: false);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    UpdateGridViewReference(e.OldItems, clear: true);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    UpdateGridViewReference(e.OldItems, clear: true);
                    UpdateGridViewReference(e.NewItems, clear: false);
                    break;
            }
        }

        private void UpdateGridViewReference(IList list, bool clear)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                GroupGridViewColumn gridViewColumn = (GroupGridViewColumn)list[i];
                if (clear)
                {
                    if (gridViewColumn.OwnerGridView == _ownerGridView)
                    {
                        gridViewColumn.OwnerGridView = null;
                    }
                    continue;
                }
                if (gridViewColumn.OwnerGridView != null && gridViewColumn.OwnerGridView != _ownerGridView)
                {
                    gridViewColumn.OwnerGridView.Columns.Remove(gridViewColumn);
                }
                gridViewColumn.OwnerGridView = _ownerGridView;
            }
        }

        internal void SetAllColumnsWidth()
        {
            _allGridStarLength = 0;
			using (IEnumerator<GroupGridViewColumn> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GroupGridViewColumn current = enumerator.Current;
                    _allGridStarLength += current.Width.Value;
				}
			}
            _perStarLength = _allGridStarLength == 0 ? 0 : _ownerGridView.ActualWidth / _allGridStarLength;
            using (IEnumerator<GroupGridViewColumn> enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    GroupGridViewColumn current = enumerator.Current;
                    DataGridLength width = current.Width;
                    current.Width = new DataGridLength(width.Value, width.UnitType, width.DesiredValue, width.Value * _perStarLength);
                }
            }
        }
    }
}
