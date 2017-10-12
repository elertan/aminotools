using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.CustomPages;
using AminoTools.Models.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.Pages.Blogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommunitySelectionPage : BasePage
    {
        public CommunitySelectionPage(Action<CommunitySelectionResult> communitySelectionResultAction) : base(communitySelectionResultAction)
        {
            InitializeComponent();
        }
    }
}