using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Providers;
using Autofac;

namespace AminoTools
{
    public class DependencyManager
    {
        public readonly IContainer Container;
        public DependencyManager()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            Container = containerBuilder.Build();
        }

        private void RegisterDependencies(ContainerBuilder cb)
        {
            cb.RegisterType<BlogProvider>().As<IBlogProvider>();
        }
    }
}
