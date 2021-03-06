﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.CustomPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.Pages.Chatting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlobalChattingPage : BasePage
    {
        public GlobalChattingPage() : base()
        {
            InitializeComponent();
        }

        public Grid GetLoadingGrid() => LoadingGrid;
    }
}
