using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Auth;
using AminoApi.Models.Blog;
using AminoApi.Models.Chat;
using AminoApi.Models.Community;
using AminoApi.Models.Feed;
using AminoApi.Models.Media;
using AminoApi.Models.User;

namespace AminoApi
{
    public interface IApi
    {
        string Version { get; set; }
        string DeviceId { get; set; }
        string Sid { get; set; }

        Task<ApiResult<Account>> LoginAsync(string email, string password);

        Task<ApiResult<CommunityList>> GetJoinedCommunitiesAsync(int start = 0, int size = 50);

        Task<ApiResult<CommunityList>> GetSuggestedCommunitiesAsync(int start = 0, int size = 50);

        Task<ApiResult<CommunityList>> GetCommuntiesByQueryAsync(string query, int start = 0, int size = 0);

        Task<ApiResult<UserProfile>> JoinAminoAsync(string id);

        Task<ApiResult<UserProfile>> GetUserProfileByIdAsync(string communityId, string userId);

        Task<ApiResult<CommunityCollectionResponse>> GetCommunityCollectionBySectionsAsync(int start = 0, int size = 25, string languageCode = "en");

        Task<ApiResult<BlogList>> GetBlogsByUserIdAsync(string communityId, string userId,
            int start = 0, int size = 25);

        Task<ApiResult<Blog>> PostBlogAsync(string communityId, string title, string content,
            IEnumerable<ImageItem> imageItems = null);

        Task<ApiResult<FeedHeadlines>> GetFeedHeadlinesAsync(int start = 0, int size = 25);

        Task<ApiResult<ImageItem>> UploadImageAsync(Stream imageStream);

        Task<ApiResult<ThreadList>> GetJoinedChatsAsync(string communityId, int start = 0, int size = 25);

        Task<ApiResult<MessageList>> GetMessagesForUserByCommunityIdAsync(string communityId, string threadId, int start = 0,
            int size = 25);

        Task<ApiResult<Message>> SendMessageToChatAsync(string communityId, string threadId,
            string content);

        Task<ApiResult<Message>> SendImageToChatAsync(string communityId, string threadId,
            string base64JpgImageData);
    }
}
