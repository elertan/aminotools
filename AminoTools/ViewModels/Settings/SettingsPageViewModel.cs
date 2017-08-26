using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoTools.ViewModels.Settings
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public SettingsPageViewModel()
        {
            Initialize += SettingsPageViewModel_Initialize;
        }

        private void SettingsPageViewModel_Initialize(object sender, EventArgs e)
        {
            
        }
    }
}
