﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoTools.Providers;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels.Contracts;
using AminoTools.ViewModels.Contracts.Auth;
using AminoTools.ViewModels.Contracts.Blogs;
using AminoTools.ViewModels.Contracts.Chatting;
using AminoTools.ViewModels.Contracts.Common;
using AminoTools.ViewModels.Contracts.Community;
using AminoTools.ViewModels.Contracts.Profile;
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
            // General
            cb.RegisterInstance(new HttpClient()).SingleInstance();

            cb.RegisterType<RandomWordProvider>().As<IRandomWordProvider>().SingleInstance();

            // Api
            cb.RegisterType<Api>().As<IApi>().SingleInstance();

            // Providers
            cb.RegisterType<BlogProvider>().As<IBlogProvider>();
            cb.RegisterType<CommunityProvider>().As<ICommunityProvider>();
            cb.RegisterType<AuthorizationProvider>().As<IAuthorizationProvider>();
            cb.RegisterType<FeedProvider>().As<IFeedProvider>();
            cb.RegisterType<MediaProvider>().As<IMediaProvider>();
            cb.RegisterType<ChatProvider>().As<IChatProvider>();
            cb.RegisterType<UserProvider>().As<IUserProvider>();

            // ViewModels
            cb.RegisterType<ViewModels.MainPageMenuPageViewModel>().As<IMainPageMenuPageViewModel>();
            cb.RegisterType<ViewModels.TestPageViewModel>().As<ITestPageViewModel>();
            cb.RegisterType<ViewModels.HomePageViewModel>().As<IHomePageViewModel>();

            // ViewModels.Common
            cb.RegisterType<ViewModels.Common.ImageSelectionPageViewModel>().As<IImageSelectionPageViewModel>();
            cb.RegisterType<ViewModels.Common.ImagePageViewModel>().As<IImagePageViewModel>();

            // ViewModels.Auth
            cb.RegisterType<ViewModels.Auth.LoginPageViewModel>().As<ILoginPageViewModel>();

            // ViewModels.Blogs
            cb.RegisterType<ViewModels.Blogs.BlogsPageViewModel>().As<IBlogsPageViewModel>();
            cb.RegisterType<ViewModels.Blogs.MultiBlogPageViewModel>().As<IMultiBlogPageViewModel>();
            cb.RegisterType<ViewModels.Blogs.CommunitySelectionPageViewModel>().As<ICommunitySelectionPageViewModel>();

            // ViewModels.Chatting
            cb.RegisterType<ViewModels.Chatting.GlobalChattingPageViewModel>().As<IGlobalChattingPageViewModel>();
            cb.RegisterType<ViewModels.Chatting.ChatPageViewModel>().As<IChatPageViewModel>();

            // ViewModels.Community
            cb.RegisterType<ViewModels.Community.CommunityPageViewModel>().As<ICommunityPageViewModel>();
            cb.RegisterType<ViewModels.Community.JoinRandomCommunitiesPageViewModel>().As<IJoinRandomCommunitiesPageViewModel>();

            // ViewModels.Profile
            cb.RegisterType<ViewModels.Profile.ProfilePageViewModel>().As<IProfilePageViewModel>();
            cb.RegisterType<ViewModels.Profile.ProfileEditPageViewModel>().As<IProfileEditPageViewModel>();

            // ViewModels.Settings
            cb.RegisterType<ViewModels.Settings.SettingsPageViewModel>().As<ISettingsPageViewModel>();
            cb.RegisterType<ViewModels.Settings.AccountPageViewModel>().As<IAccountPageViewModel>();
        }
    }
}