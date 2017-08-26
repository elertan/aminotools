using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.CustomControls;
using AminoTools.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.CustomPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasePage : ContentPage
    {
        public View ParentPageContent { get; set; }
        public BaseViewModel BaseViewModel { get; set; }
        public BasePage()
        {
            InitializeComponent();
        }

        private void BasePage_OnAppearing(object sender, EventArgs e)
        {
            if (ParentPageContent == null) ParentPageContent = Content;

            // Get ViewModel
            BaseViewModel = (BaseViewModel)BindingContext;
            BaseViewModel.Page = this;
            BaseViewModel.PropertyChanged += BaseViewModel_PropertyChanged;

            UpdateLoadingOverlay();

            BaseViewModel.OnInitialize();
        }

        private void BaseViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaseViewModel.IsBusy))
            {
                UpdateLoadingOverlay();
            }
        }

        private void UpdateLoadingOverlay()
        {
            if (BaseViewModel.IsBusy)
            {
                // Add LoadingOverlay
                var grid = new Grid();
                var loadingOverlay = new LoadingOverlay();
                grid.Children.Add(ParentPageContent);
                grid.Children.Add(loadingOverlay);

                Content = grid;
            }
            else
            {
                Content = ParentPageContent;
            }
        }
    }
}