using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFDemo.SimpleFrame.Container.IViewModels;
using WPFDemo.SimpleFrame.Infra.Enums;
using WPFDemo.SimpleFrame.Infra.Helper;
using WPFDemo.SimpleFrame.Infra.MVVM;

namespace WPFDemo.SimpleFrame.Container.ViewModels
{
    public class NaviPageViewModel : BaseViewModel, INaviPageViewModel
    {
        private List<EnumStructInfo> _pageNaviSource;
        private EnumStructInfo _selectedNavi;
        private Uri _naviSource;
        public List<EnumStructInfo> PageNaviSource
        {
            get => _pageNaviSource;
            set
            {
                _pageNaviSource = value;
                OnPropertyChanged(() => PageNaviSource);
            }
        }
        public Uri NaviSource
        {
            get => _naviSource;
            set
            {
                _naviSource = value;
                OnPropertyChanged(() => NaviSource);
            }
        }

        public EnumStructInfo SelectedNavi
        {
            get => _selectedNavi;
            set
            {
                _selectedNavi = value;
                OnNaviChanged(_selectedNavi);
            }
        }

        public NaviPageViewModel()
        {

        }

        private void OnNaviChanged(EnumStructInfo selectedNavi)
        {
            if(selectedNavi != null)
            {
                NaviSource = new Uri(@"NaviPages\" + selectedNavi.Description, UriKind.RelativeOrAbsolute);
            }
        }

        protected override void Loaded()
        {
            PageNaviSource = typeof(NaviKeyEnum).GetEnumStructInfo();
            OnNaviChanged(PageNaviSource.First());
        }

        protected override void UnLoaded()
        {
            
        }
    }
}
