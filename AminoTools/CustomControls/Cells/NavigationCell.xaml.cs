using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.CustomControls.Cells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavigationCell : ViewCell, INotifyPropertyChanged
	{
	    private string _text;
	    private Type _targetPageType;
	    private Command _cellTappedCommand;

	    public string Text
	    {
	        get => _text;
	        set
	        {
	            _text = value; 
	            OnPropertyChanged();
	        }
	    }

	    public Type TargetPageType
	    {
	        get => _targetPageType;
	        set
	        {
	            _targetPageType = value; 
	            OnPropertyChanged();
	        }
	    }

	    public Command CellTappedCommand
	    {
	        get => _cellTappedCommand;
	        set
	        {
                if (_cellTappedCommand != null) throw new NotSupportedException("You should not set this value, since it's used to handle the navigation");
	            _cellTappedCommand = value; 
	            OnPropertyChanged();
	        }
	    }

	    public NavigationCell ()
		{
			InitializeComponent ();
		    StyleId = "disclosure";

            CellTappedCommand = new Command(DoNavigate);
		}

	    private async void DoNavigate()
	    {
	        var page = (Page)Activator.CreateInstance(TargetPageType);
	        await ((App) Application.Current).MainNavigation.PushAsync(page, true);
	    }
	}
}