using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Blog;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Blogs
{
    public interface IMultiBlogPageViewModel
    {
        IEnumerable<Stream> ImageStreams { get; set; }
        Blog Blog { get; set; }
        Command ImagesCommand { get; set; }
        Command NextCommand { get; set; }
    }
}
