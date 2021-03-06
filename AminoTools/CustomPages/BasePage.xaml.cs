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
        private readonly object _viewModelParameter;
        private bool _firstTimeAppearing = true;
        public View ParentPageContent { get; set; }
        public BaseViewModel BaseViewModel { get; set; }
        public BasePage(object viewModelParameter = null)
        {
            _viewModelParameter = viewModelParameter;
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            if (BaseViewModel.IsInitializing) BaseViewModel.OnCancelledInitialization();
            BaseViewModel.WantsDisposal = true;

            return base.OnBackButtonPressed();
        }

        private void BasePage_OnAppearing(object sender, EventArgs e)
        {
            if (!_firstTimeAppearing) return;
            _firstTimeAppearing = false;

            if (ParentPageContent == null) ParentPageContent = Content;

            if (BaseViewModel == null)
            {
                // Get ViewModel
                if (BindingContext is ViewModelResolver)
                {
                    var resolver = (ViewModelResolver) BindingContext;
                    BaseViewModel = (BaseViewModel) resolver.Model;
                }
                else
                {
                    BaseViewModel = (BaseViewModel) BindingContext;
                }
                BaseViewModel.ViewModelParameter = _viewModelParameter;
            }

            BaseViewModel.Page = this;
            BaseViewModel.IsBusyData.PropertyChanged += IsBusyData_PropertyChanged;

            UpdateLoadingOverlay();

            BaseViewModel.OnInitialize();
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