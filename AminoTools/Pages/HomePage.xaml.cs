﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.CustomPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.Pages
{
	[XamlCompilation(XamlCompilationOptions.Skip)]
	public partial class HomePage : BasePage
	{
		public HomePage () : base()
		{
			InitializeComponent ();
		}
	}
}