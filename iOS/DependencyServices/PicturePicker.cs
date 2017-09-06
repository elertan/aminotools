using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AminoTools.DependencyServices;
using AminoTools.iOS.DependencyServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PicturePicker))]
namespace AminoTools.iOS.DependencyServices
{
    public class PicturePicker : IPicturePicker
    {
        private TaskCompletionSource<Stream> _taskCompletionSource;
        private UIImagePickerController _imagePickerController;

        public Task<Stream> GetImageStreamAsync()
        {
            _imagePickerController = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
            };

            // In case of a simulator, because a camera doesn't exist
            if (!UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.PhotoLibrary))
            {
                _imagePickerController.SourceType = UIImagePickerControllerSourceType.SavedPhotosAlbum;
            }

            _imagePickerController.FinishedPickingMedia += _imagePickerController_FinishedPickingMedia;
            _imagePickerController.Canceled += _imagePickerController_Canceled;

            var window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.PresentModalViewController(_imagePickerController, true);

            _taskCompletionSource = new TaskCompletionSource<Stream>();
            return _taskCompletionSource.Task;
        }

        private void _imagePickerController_Canceled(object sender, EventArgs e)
        {
            _taskCompletionSource.SetResult(null);
            _imagePickerController.DismissModalViewController(true);
        }

        private void _imagePickerController_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            var image = e.EditedImage ?? e.OriginalImage;
            if (image != null)
            {
                var data = image.AsJPEG(1);
                var stream = data.AsStream();

                _taskCompletionSource.SetResult(stream);
            }
            else
            {
                _taskCompletionSource.SetResult(null);
            }
            _imagePickerController.DismissModalViewController(true);
        }
    }
}
