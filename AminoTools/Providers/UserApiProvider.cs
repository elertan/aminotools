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
    public class UserApiProvider : ApiProvider, IUserProvider
    {
        public async Task<ApiResult<UserProfile>> GetUserProfileByIdAsync(string communityId, string userId)
        {
            return await Api.GetUserProfileByIdAsync(communityId, userId);
        }

        public UserApiProvider(IApi api) : base(api)
        {
        }
    }
}
