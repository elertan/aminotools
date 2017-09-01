using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;

namespace AminoTools.Droid
{
    [Activity(Label = "AminoTools.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private App _app;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CachedImageRenderer.Init();

            _app = new App();
            LoadApplication(_app);
        }

        public override void OnBackPressed()
        {
            // Prevent back from being pressed in certain scenario's
            if (_app.CurrentViewModel.IsBusyData.IsBusy || _app.MainNavigation.NavigationStack.Count == 1)
            {
                return;
            }

            base.OnBackPressed();
        }
    }
}
