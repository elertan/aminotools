using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Models.Common.ImageSelection;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Common
{
    public interface IImageSelectionPageViewModel
    {
        Command AddNewImageCommand { get; }
        Command RemoveImageCommand { get; }

        ObservableCollection<BlogImageSource> BlogImageSources { get; set; }
    }
}
