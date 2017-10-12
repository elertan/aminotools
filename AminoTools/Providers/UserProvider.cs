using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.User;
using AminoTools.Providers.Contracts;

namespace AminoTools.Providers
{
    public class UserProvider : Provider, IUserProvider
    {
        public async Task<ApiResult<UserProfile>> GetUserProfileByIdAsync(string communityId, string userId)
        {
            return await Api.GetUserProfileByIdAsync(communityId, userId);
        }

        public UserProvider(IApi api) : base(api)
        {
        }
    }
}
