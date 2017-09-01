using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingOverlay : ContentView
    {
        private BaseViewModel _baseViewModel;

        public BaseViewModel BaseViewModel
        {
            get => _baseViewModel;
            set
            {
                _baseViewModel = value; 
                OnPropertyChanged();
            }
        }

        public LoadingOverlay(BaseViewModel bvm)
        {
            BaseViewModel = bvm;
            try
            {
                InitializeComponent();
            }
            catch (NullReferenceException ex)
            {
                
            }
        }
    }
}