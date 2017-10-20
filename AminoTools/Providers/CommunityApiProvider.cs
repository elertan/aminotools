using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Community;
using AminoApi.Models.User;
using AminoTools.Providers.Contracts;
using SQLiteNetExtensionsAsync.Extensions;
using Xamarin.Forms;

namespace AminoTools.Providers
{
    public class CommunityApiProvider : ApiProvider, ICommunityProvider
    {
        private readonly IDatabaseProvider _databaseProvider;
        private readonly IMediaProvider _mediaProvider;

        public CommunityApiProvider(IApi api, 
            IDatabaseProvider databaseProvider,
            IMediaProvider mediaProvider) : base(api)
        {
            _databaseProvider = databaseProvider;
            _mediaProvider = mediaProvider;
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

        public async Task StoreCommunitiesAsync(params Community[] communities)
        {
            var db = await _databaseProvider.GetDatabaseAsync();
            //foreach (var community in communities)
            //{
            //    await _mediaProvider.StoreImageItemsAsync(community.ImageItems);
            //}
            await db.Connection.InsertOrReplaceAllWithChildrenAsync(communities, true);
        }

        public async Task<List<Community>> GetStoredCommunitiesAsync()
        {
            var db = await _databaseProvider.GetDatabaseAsync();
            return await db.Connection.GetAllWithChildrenAsync<Community>(null, true);
        }

        public async Task<Community> GetStoredCommunityAsync(string communityId)
        {
            var db = await _databaseProvider.GetDatabaseAsync();
            return await db.Connection.GetWithChildrenAsync<Community>(null, true);
        }
    }
}
