using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Ioc
{
    public class IocManagerInstance
    {
        private static IocManager _iocManager;

        public static void Init(IocManager iocManager)
        {
            _iocManager = iocManager;
        }

        public static TService ResolveType<TService>()
        {
            return _iocManager.ResolveType<TService>();
        }
    }
}
