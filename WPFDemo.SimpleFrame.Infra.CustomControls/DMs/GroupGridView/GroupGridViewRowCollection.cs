using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.CustomControls.DMs
{
    internal class GroupGridViewRowCollection : ObservableCollection<GroupGridViewRow>
    {
        private readonly GroupGridView _ownerGridView;
        public GroupGridViewRowCollection(GroupGridView gridView)
        {
            _ownerGridView = gridView;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    UpdateGridViewRowIndex();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    UpdateGridViewRowIndex();
                    break;
            }
        }

        private void UpdateGridViewRowIndex()
        {
            using (IEnumerator<GroupGridViewRow> enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    GroupGridViewRow current = enumerator.Current;
                    current.GroupGridViewRowIndex = IndexOf(current);
                    current.GroupGridViewRowAlternationIndex = _ownerGridView.AlternationCount <= 0 ? 0 : current.GroupGridViewRowIndex % _ownerGridView.AlternationCount;
                }
            }
        }

        internal void InsertRow(int index, GroupGridViewRow row)
        {
            if (!Contains(row))
            {
                Insert(index, row);
            }
        }

        internal void RemoveRow(GroupGridViewRow row)
        {
            if(Contains(row))
            {
                Remove(row);
            }
        }
    }
}
