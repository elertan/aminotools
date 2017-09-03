using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models;
using AminoApi.Models.Auth;
using AminoApi.Models.Blog;
using AminoApi.Models.Community;
using Xamarin.Forms;
using AminoTools.Pages;
using AminoTools.Providers.Contracts;
using AminoTools.ViewModels;
using AminoTools.ViewModels.Auth;
using Autofac;
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

        public App()
        {
            DependencyManager = new DependencyManager();
            Variables = new VariablesClass();
            Api = DependencyManager.Container.Resolve<IApi>();

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
                var email = SettingsManager.GetSetting<string>(SettingsManager.AvailableSettings.Username);
                var password = SettingsManager.GetSetting<string>(SettingsManager.AvailableSettings.Password);

                // Retry login
                var provider = DependencyManager.Container.Resolve<IAuthorizationProvider>();
                var acc = await provider.Login(email, password);

                await Login(acc);
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
            }

            public readonly MultiBlogClass MultiBlog;
            public class MultiBlogClass
            {
                public Blog Blog { get; set; }
                public IEnumerable<Community> Communities { get; set; }
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
