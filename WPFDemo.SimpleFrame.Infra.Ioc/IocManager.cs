using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Ioc
{
    public class IocManager
    {
        private static readonly ContainerBuilder _builder = new ContainerBuilder();

        private static IContainer _container;

        public static void RegisterAssemblyInterfaces(string[] assemblys)
        {
            foreach (var assembly in assemblys)
            {
                var cc = _builder.RegisterAssemblyTypes(Assembly.Load(assembly)).AsImplementedInterfaces();
            }
            _container = _builder.Build();
        }

        public TService ResolveType<TService>()
        {
            return _container.Resolve<TService>();
        }
    }
}
