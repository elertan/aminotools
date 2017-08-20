using Xamarin.Forms;

namespace AminoTools
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AminoToolsPage();
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
