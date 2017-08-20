using System.Net.Http;
using AminoApi;
using Xamarin.Forms;
using AminoTools.Pages;

namespace AminoTools
{
    public partial class App : Application
    {
        public Api Api { get; set; } = new Api(new HttpClient());

        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
