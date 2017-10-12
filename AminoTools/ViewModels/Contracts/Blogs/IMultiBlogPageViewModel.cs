using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Blog;
using AminoTools.Models.Common.ImageSelection;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Blogs
{
    public interface IMultiBlogPageViewModel
    {
        Blog Blog { get; set; }
        Command ImagesCommand { get; set; }
        Command NextCommand { get; set; }
        ObservableCollection<BlogImageSource> BlogImageSources { get; set; }
    }
}
