﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AminoTools.ViewModels.Contracts.Community
{
    public interface IJoinRandomCommunitiesPageViewModel
    {
        Command JoinCommunitiesCommand { get; }
    }
}
