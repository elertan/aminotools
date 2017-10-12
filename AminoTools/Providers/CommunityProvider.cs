using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Community;
using AminoApi.Models.User;
using AminoTools.Providers.Contracts;
using Xamarin.Forms;

namespace AminoTools.Providers
{
    public class CommunityProvider : Provider, ICommunityProvider
    {
        public CommunityProvider(IApi api) : base(api)
        {
        }

        public async Task<ApiResult<IEnumerable<Community>>> GetJoinedCommunities(int index = 0, int amount = 50)
        {
            var result = await Api.GetJoinedCommunitiesAsync(index, amount);
            return ApiResult.Create((IEnumerable<Community>)result.Data.Communities, result.Info);
        }

        public async Task<ApiResult<IEnumerable<Community>>> GetAllJoinedCommunities()
        {
            var list = new List<Community>();

            for (var i = 0;; i++)
            {
                var result = await GetJoinedCommunities(50 * i);
                if (!result.DidSucceed()) return result;
                if (!result.Data.Any()) break;
                list.AddRange(result.Data);
            }

            return ApiResult.Create((IEnumerable<Community>)list);
        }

        public async Task<ApiResult<IEnumerable<Community>>> GetSuggestedCommunities(int index = 0, int amount = 50)
        {
            var result = await Api.GetSuggestedCommunitiesAsync(index, amount);
            return ApiResult.Create((IEnumerable<Community>) result.Data.Communities, result.Info);
        }

        public async Task<ApiResult<IEnumerable<Community>>> GetCommunitiesByQuery(string query, int index = 0, int amount = 25)
        {
            var result = await Api.GetCommuntiesByQueryAsync(query, index, amount);
            return ApiResult.Create((IEnumerable<Community>) result.Data.Communities, result.Info);
        }

        public async Task<ApiResult<CommunityCollectionResponse>> GetCommunitiesFromExplore(int index = 0, int amount = 25)
        {
            var result = await Api.GetCommunityCollectionBySectionsAsync(index, amount);
            return result;
        }

        public async Task<ApiResult<UserProfile>> JoinCommunity(string id)
        {
            var result = await Api.JoinAminoAsync(id);
            return result;
        }
    }
}
