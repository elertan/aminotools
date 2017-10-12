using System.Collections.Generic;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Community;
using AminoApi.Models.User;

namespace AminoTools.Providers.Contracts
{
    public interface ICommunityProvider
    {
        Task<ApiResult<IEnumerable<Community>>> GetJoinedCommunities(int index = 0, int amount = 50);
        Task<ApiResult<IEnumerable<Community>>> GetAllJoinedCommunities();
        Task<ApiResult<IEnumerable<Community>>> GetSuggestedCommunities(int index = 0, int amount = 50);
        Task<ApiResult<IEnumerable<Community>>> GetCommunitiesByQuery(string query, int index = 0, int amount = 25);
        Task<ApiResult<CommunityCollectionResponse>> GetCommunitiesFromExplore(int index = 0, int amount = 25);
        Task<ApiResult<UserProfile>> JoinCommunity(string id);
    }
}
