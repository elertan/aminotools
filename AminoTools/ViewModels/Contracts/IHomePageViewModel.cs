using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;
using AminoApi.Models.Blog;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts
{
    interface IHomePageViewModel
    {
        Command TestButtonCommand { get; set; }
        IEnumerable<Blog> Blogs { get; set; }
    }
}
