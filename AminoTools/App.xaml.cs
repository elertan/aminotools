﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models;
using AminoApi.Models.Auth;
using AminoApi.Models.Blog;
using AminoApi.Models.Community;
using AminoApi.Models.User;
using AminoTools.DependencyServices;
using AminoTools.Models.Common.ImageSelection;
using Xamarin.Forms;
using AminoTools.Pages;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels;
using AminoTools.ViewModels.Auth;
using Autofac;
using FFImageLoading;
using FFImageLoading.Config;
using SQLite;
using SQLite.Net.Async;
using Xamarin.Forms.Xaml;
using LoginPage = AminoTools.Pages.Auth.LoginPage;

// Compile all xaml pages
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AminoTools
{
    public partial class App : Application
    {
        private INavigation _mainNavigation;

        public INavigation MainNavigation
        {
            get => _mainNavigation ?? ((App) Current).MainPage.Navigation;
            set => _mainNavigation = value;
        }

        public readonly DependencyManager DependencyManager;

        public Account Account { get; protected set; }
        public IApi Api { get; protected set; }
        public BaseViewModel CurrentViewModel { get; set; }
        public MasterDetailPage MasterDetailPage { get; protected set; }
        public SQLiteAsyncConnection DbConnection { get; set; }

        public App()
        {
            DependencyManager = new DependencyManager();
            Variables = new VariablesClass();
            Api = DependencyManager.Container.Resolve<IApi>();

            var databaseService = DependencyService.Get<IDatabaseService>();
            var connection = databaseService.GetConnection();
            DbConnection = connection;

            var httpClient = new HttpClient(); // this handler will not throw if hostname is different
            ImageService.Instance.Initialize(new Configuration() { HttpClient = httpClient, ExecuteCallbacksOnUIThread = false, AnimateGifs = false});

            InitializeComponent();

            MainPage = new LoadingPage();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts

            var exception = await ExceptionManager.GetExceptionAsync();
            if (exception != null)
            {
                MainPage = new ExceptionPage(exception);
                return;
            }

            await StartUp();
        }

        public async Task StartUp()
        {
            // Already logged in
            var account = SettingsManager.GetSettingOrDefault<Account>(SettingsManager.AvailableSettings.Account);
            if (account != null)
            {
                var email = SettingsManager.GetSettingWithFallback(SettingsManager.AvailableSettings.Username, String.Empty);
                var password = SettingsManager.GetSettingWithFallback(SettingsManager.AvailableSettings.Password, String.Empty);

                if (string.IsNullOrWhiteSpace(email)
                    || string.IsNullOrWhiteSpace(password))
                {
                    // Yet to log in
                    MainPage = new NavigationPage(new LoginPage());
                    return;
                }

                // Retry login
                var provider = DependencyManager.Container.Resolve<IAuthorizationProvider>();
                var acc = await provider.Login(email, password);

                await Login(acc.Data);
                return;
            }

            // Yet to log in
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


        public readonly VariablesClass Variables;
        public class VariablesClass
        {
            public VariablesClass()
            {
                MultiBlog = new MultiBlogClass();
                ImageSelection = new ImageSelectionClass();
                ProfilePage = new UserProfileClass();
                ProfileEditPage = new ProfileEditPageClass();
            }

            public readonly MultiBlogClass MultiBlog;
            public class MultiBlogClass
            {
                public Blog Blog { get; set; }
                public IEnumerable<Community> Communities { get; set; }
                public List<BlogImageSource> BlogImageSources { get; set; }
            }

            public readonly ImageSelectionClass ImageSelection;
            public class ImageSelectionClass
            {
                public ObservableCollection<BlogImageSource> BlogImageSources { get; set; }
                public event EventHandler UpdatedImages;

                public virtual void OnUpdatedImages()
                {
                    UpdatedImages?.Invoke(this, EventArgs.Empty);
                }
            }

            public readonly UserProfileClass ProfilePage;
            public class UserProfileClass
            {
                public string CommunityId { get; set; }
                public string UserId { get; set; }
                public bool IsMyProfile { get; set; }

                public void Reset()
                {
                    CommunityId = null;
                    UserId = null;
                    IsMyProfile = false;
                }
            }

            public readonly ProfileEditPageClass ProfileEditPage;

            public class ProfileEditPageClass
            {
                public UserProfile Profile { get; set; }
            }
        }

        public void GoToStartPage()
        {
            var page = new MainPage();
            MasterDetailPage = page;
            MainNavigation = page.Detail.Navigation;
            MainPage = page;
        }

        public async Task SetMainPage(Page page)
        {
            await MainNavigation.PopToRootAsync(false);
            var masterDetailPage = (MasterDetailPage) MainPage;
            var currentPage = ((NavigationPage) masterDetailPage.Detail).CurrentPage;
            MainNavigation.InsertPageBefore(page, currentPage);
            await MainNavigation.PopToRootAsync(false);
            if (masterDetailPage.IsPresented) masterDetailPage.IsPresented = false;
        }

        public async Task Logout()
        {
            Account = null;
            Api.Sid = null;
            await SettingsManager.ClearSettingAsync(SettingsManager.AvailableSettings.Account);

            MainPage = new NavigationPage(new LoginPage());
        }

        public async Task Login(Account account)
        {
            await SettingsManager.SaveComplexSettingAsync(SettingsManager.AvailableSettings.Account, account);

            Api.Sid = account.Sid;
            Account = account;

            GoToStartPage();
        }

        // MAY NOT BE ASYNC
        public void ExceptionOccured(object sender, Exception ex)
        {
            if (ex != null)
            {
                ExceptionManager.ReportException(ex);
            }
        }
    }
}
