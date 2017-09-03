using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Community;
using AminoApi.Models.User;

namespace AminoTools.Providers.Contracts
{
    public interface ICommunityProvider
    {
        Task<IEnumerable<Community>> GetJoinedCommunities(int index = 0, int amount = 50);
        Task<IEnumerable<Community>> GetSuggestedCommunities(int index = 0, int amount = 50);
        Task<IEnumerable<Community>> GetCommunitiesByQuery(string query, int index = 0, int amount = 25);
        Task<CommunityCollectionResponse> GetCommunitiesFromExplore(int index = 0, int amount = 25);
        Task<UserProfile> JoinCommunity(string id);
    }
}
