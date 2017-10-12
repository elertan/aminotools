using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;
using AminoApi.Models.Blog;
using AminoApi.Models.Feed;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts
{
    public interface IHomePageViewModel
    {
        IEnumerable<HeadLineBlog> Blogs { get; set; }
    }
}
