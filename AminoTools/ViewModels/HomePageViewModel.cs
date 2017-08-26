using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoTools.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
            Initialize += HomePageViewModel_Initialize;
        }

        private void HomePageViewModel_Initialize(object sender, EventArgs e)
        {
            
        }
    }
}
