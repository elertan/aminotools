using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Providers;
using AminoTools.ViewModels.Contracts;
using AminoTools.ViewModels.Contracts.Auth;
using AminoTools.ViewModels.Contracts.Settings;
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
            // Providers
            cb.RegisterType<BlogProvider>().As<IBlogProvider>();

            // ViewModels
            cb.RegisterType<ViewModels.MainPageMenuPageViewModel>().As<IMainPageMenuPageViewModel>();
            cb.RegisterType<ViewModels.TestPageViewModel>().As<ITestPageViewModel>();
            cb.RegisterType<ViewModels.HomePageViewModel>().As<IHomePageViewModel>();

            // ViewModels.Auth
            cb.RegisterType<ViewModels.Auth.LoginPageViewModel>().As<ILoginPageViewModel>();

            // ViewModels.Settings
            cb.RegisterType<ViewModels.Settings.SettingsPageViewModel>().As<ISettingsPageViewModel>();
            cb.RegisterType<ViewModels.Settings.AccountPageViewModel>().As<IAccountPageViewModel>();
        }
    }
}