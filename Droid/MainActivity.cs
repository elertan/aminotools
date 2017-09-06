using System;
using System.IO;
using AminoTools.Droid.DependencyServices;
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
    [Activity(Label = "AminoTools", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private App _app;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            // Exception Handling
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CachedImageRenderer.Init();

            _app = new App();
            LoadApplication(_app);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            switch (requestCode)
            {
                case (int)RequestCodes.PicturePicker:
                    if (resultCode == Result.Ok && intent != null)
                    {
                        var uri = intent.Data;
                        var stream = ContentResolver.OpenInputStream(uri);

                        // Set the Stream as the completion of the Task
                        PicturePicker.TaskCompletionSource.SetResult(stream);
                    }
                    else
                    {
                        PicturePicker.TaskCompletionSource.SetResult(null);
                    }
                    break;
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _app.ExceptionOccured(sender, e.ExceptionObject as Exception);
        }

        public override void OnBackPressed()
        {
            // Prevent back from being pressed in certain scenario's
            if (_app.CurrentViewModel.IsBusyData.IsBusy)
            {
                return;
            }
            if (_app.MainNavigation.NavigationStack.Count == 1)
            {
                if (!_app.MasterDetailPage.IsPresented) _app.MasterDetailPage.IsPresented = true;
                return;
            }

            base.OnBackPressed();
        }
    }
}
