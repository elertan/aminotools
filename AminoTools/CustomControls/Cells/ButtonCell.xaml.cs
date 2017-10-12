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
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ButtonCell : ViewCell, INotifyPropertyChanged
	{
	    private string _text;
	    private Color _textColor;

        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(Command), typeof(ButtonCell));

        public string Text
	    {
	        get => _text;
	        set
	        {
	            _text = value;
	            OnPropertyChanged();
	        }
	    }
        public Command Command
	    {
	        get => (Command)GetValue(CommandProperty);
	        set => SetValue(CommandProperty, value);
	    }

	    public Color TextColor
	    {
	        get => _textColor;
	        set
	        {
	            _textColor = value;
	            OnPropertyChanged();
	        }
	    }

	    public ButtonCell ()
		{
			InitializeComponent ();

            // Default Color
            TextColor = Color.Black;
		}
	}
}