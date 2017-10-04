using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.User;

namespace AminoTools.Providers.Contracts
{
    public interface IUserProvider
    {
        Task<ApiResult<UserProfile>> GetUserProfileByIdAsync(string communityId, string userId);
    }
}
