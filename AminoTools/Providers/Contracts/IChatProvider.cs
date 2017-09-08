﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Chat;

namespace AminoTools.Providers.Contracts
{
    public interface IChatProvider
    {
        Task<ApiResult<List<Chat>>> GetChatsByCommunityAsync(string communityId);
    }
}