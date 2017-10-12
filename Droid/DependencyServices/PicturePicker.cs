using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.DependencyServices;
using AminoTools.Droid.DependencyServices;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(PicturePicker))]
namespace AminoTools.Droid.DependencyServices
{
    public class PicturePicker : IPicturePicker
    {
        public static TaskCompletionSource<Stream> TaskCompletionSource { set; get; }

        public Task<Stream> GetImageStreamAsync()
        {
            // Define the Intent for getting images
            var intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            // Get the MainActivity instance
            var activity = (MainActivity) Forms.Context;

            // Start the picture-picker activity (resumes in MainActivity.cs)
            activity.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                (int)RequestCodes.PicturePicker);

            // Save the TaskCompletionSource object as a MainActivity property
            TaskCompletionSource = new TaskCompletionSource<Stream>();

            // Return Task object
            return TaskCompletionSource.Task;
        }
    }
}