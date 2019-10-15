using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDemo.SimpleFrame.Infra.MVVM.VMOnly
{
    public abstract class DataPagerQueryViewModel<T> : BaseViewModel
    {
        private List<T> _dataSource;
        private int _pageNo;
        private int _pageSize;
        private int _itemCount;
        private int[] _pageSizeSource;

        public int[] PageSizeSource
        {
            get => _pageSizeSource;
            set
            {
                _pageSizeSource = value;
                OnPropertyChanged(() => PageSizeSource);
            }
        }

        public List<T> DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                OnPropertyChanged(() => DataSource);
            }
        }

        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                _itemCount = value;
                OnPropertyChanged(() => ItemCount);
            }
        }
        public int PageNo
        {
            get { return _pageNo; }
            set
            {
                if (_pageNo != value)
                {
                    _pageNo = value;
                    PageSearch(_pageSize, _pageNo).GetAwaiter();
                    OnPropertyChanged(() => PageNo);
                }
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    int pageCount = (ItemCount - 1) / Math.Max(1, value) + 1;
                    if (PageNo <= pageCount)
                    {
                        PageSearch(_pageSize, 1).GetAwaiter();
                    }
                    OnPropertyChanged(() => PageSize);
                }
            }
        }

        public DataPagerQueryViewModel()
        {
            _pageNo = 1;
            _pageSize = 5;
            _dataSource = new List<T>();
            _pageSizeSource = new int[] { 5, 10, 20 };
        }

        protected abstract Task PageSearch(int pageSize, int pageNo);

        protected override abstract Task Loaded();

        protected override abstract Task UnLoaded();
    }
}
