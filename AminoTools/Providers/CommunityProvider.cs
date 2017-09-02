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
        public CommunityProvider(IApi api) : base(api)
        {
        }

        public async Task<IEnumerable<Community>> GetJoinedCommunities(int index = 0, int amount = 50)
        {
            var result = await Api.GetJoinedCommunities(index, amount);
            return result.Data.Communities;
        }

        public async Task<CommunityCollectionResponse> GetCommunitiesFromExplore(int index = 0, int amount = 25)
        {
            var result = await Api.GetCommunityCollectionBySections(index, amount);
            return result.Data;
        }
    }
}
