﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.CustomControls;
using AminoTools.Models;
using AminoTools.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.CustomPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasePage : ContentPage
    {
        private bool _hasInitialized;

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
            var resolver = (ViewModelResolver)BindingContext;
            BaseViewModel = (BaseViewModel)resolver.Model;
            BaseViewModel.Page = this;
            BaseViewModel.IsBusyData.PropertyChanged += IsBusyData_PropertyChanged;

            UpdateLoadingOverlay();

            if (!_hasInitialized) BaseViewModel.OnInitialize();
            _hasInitialized = true;
        }

        private void IsBusyData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsBusyData.IsBusy))
            {
                UpdateLoadingOverlay();
            }
        }

        private void UpdateLoadingOverlay()
        {
            if (BaseViewModel.IsBusyData.IsBusy)
            {
                // Add LoadingOverlay
                var grid = new Grid();
                var loadingOverlay = new LoadingOverlay(BaseViewModel);

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