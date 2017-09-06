using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.ViewModels.Contracts;
using AminoTools.ViewModels.Contracts.Auth;
using AminoTools.ViewModels.Contracts.Blogs;
using AminoTools.ViewModels.Contracts.Common;
using AminoTools.ViewModels.Contracts.Community;
using AminoTools.ViewModels.Contracts.Settings;
using Autofac;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public static class ViewModels
    {
        private static IContainer Container => ((App) Application.Current).DependencyManager.Container;

        // ViewModels
        public static IHomePageViewModel HomePageViewModel => Container.Resolve<IHomePageViewModel>();
        public static IMainPageMenuPageViewModel MainPageMenuPageViewModel => 
            Container.Resolve<IMainPageMenuPageViewModel>();

        public static ITestPageViewModel TestPageViewModel => Container.Resolve<ITestPageViewModel>();

        // ViewModels.Auth
        public static ILoginPageViewModel LoginPageViewModel => Container.Resolve<ILoginPageViewModel>();

        // ViewModels.Blogs
        public static IBlogsPageViewModel BlogsPageViewModel => Container.Resolve<IBlogsPageViewModel>();

        public static ICommunitySelectionPageViewModel CommunitySelectionPageViewModel => Container
            .Resolve<ICommunitySelectionPageViewModel>();

        public static IMultiBlogPageViewModel MultiBlogPageViewModel => Container.Resolve<IMultiBlogPageViewModel>();

        // ViewModels.Common
        public static IImageSelectionPageViewModel ImageSelectionPageViewModel => Container
            .Resolve<IImageSelectionPageViewModel>();

        // ViewModels.Community
        public static ICommunityPageViewModel CommunityPageViewModel => Container.Resolve<ICommunityPageViewModel>();

        public static IJoinRandomCommunitiesPageViewModel JoinRandomCommunitiesPageViewModel => Container
            .Resolve<IJoinRandomCommunitiesPageViewModel>();

        // ViewModels.Settings
        public static IAccountPageViewModel AccountPageViewModel => Container.Resolve<IAccountPageViewModel>();
        public static ISettingsPageViewModel SettingsPageViewModel => Container.Resolve<ISettingsPageViewModel>();
    }
}
