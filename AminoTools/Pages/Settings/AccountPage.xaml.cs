using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.CustomPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.Pages.Settings
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountPage : BasePage
	{
		public AccountPage () : base()
		{
			InitializeComponent ();
		}
	}
}