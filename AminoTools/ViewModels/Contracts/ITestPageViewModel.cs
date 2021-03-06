﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts
{
    public interface ITestPageViewModel
    {
        Command ButtonCommand { get; set; }
        Command DestroyDatabaseCommand { get; set; }
        Command DatabaseTestCommand { get; }
    }
}
