using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Blog;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Blogs
{
    public interface IMultiBlogPageViewModel
    {
        Blog Blog { get; set; }
        Command NextCommand { get; set; }
    }
}
