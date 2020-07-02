using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    internal class GroupGridViewColumnCollection : ObservableCollection<GroupGridViewColumn>
    {
        private GroupGridView _ownerGridView;
        private double _allGridStarLength;
        private double _perStarLength;
        internal GroupGridViewColumnCollection(GroupGridView groupGridView)
        {
            _ownerGridView = groupGridView;
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
