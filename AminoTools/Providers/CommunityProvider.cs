﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Community;
using AminoTools.Providers.Contracts;
using Xamarin.Forms;

namespace AminoTools.Providers
{
    public class CommunityProvider : Provider, ICommunityProvider
    {
        public CommunityProvider() : base(((App) Application.Current).Api)
        {
        }

        public async Task<IEnumerable<Community>> GetJoinedCommunities(int index = 0, int amount = 50)
        {
            var result = await Api.GetJoinedCommunities(index, amount);
            return result.Data.Communities;
        }
    }
}