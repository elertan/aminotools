using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExceptionPage : ContentPage
	{
	    public Exception Exception { get; }

	    public Command OkCommand { get; }

	    public ExceptionPage (Exception exception)
	    {
	        Exception = exception;
	        OkCommand = new Command(DoOk);
	        InitializeComponent ();
	    }

	    private async void DoOk()
	    {
	        ExceptionManager.ClearExceptionAsync();

            var app = (App) Application.Current;
            await app.StartUp();
	    }
	}
}