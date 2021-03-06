﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.CustomPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.Pages.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : BasePage
    {
        public ImagePage(Uri imageUri) : base()
        {
            ImageUri = imageUri;

            InitializeComponent();
        }

        public Uri ImageUri { get; set; }
    }
}