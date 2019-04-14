using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json.Linq;

namespace AminoApi
{
    public class Api : IApi
    {
        private readonly HttpInteractor _httpInteractor;
        private readonly ApiResultBuilder _apiResultBuilder;
        private string _sid;
        private const string HostAddress = "https://service.narvii.com";
        private const string BaseUrl = "/api";
        private const string SidAuthHeaderName = "NDCAUTH";
        public string Version { get; set; } = "v1";

        public string DeviceId { get; set; } =
            "01D5ED5BE317F883719728B66E5D9D7A21BF3596050F95AAB9BC707D2D82AF6F82DFDEF9B1CF305A90";

        public string Sid
        {
            get => _sid;
            set
            {
                _sid = value;
                _httpInteractor.RemoveDefaultRequestHeader(SidAuthHeaderName);
                _httpInteractor.AddDefaultRequestHeader(SidAuthHeaderName, $"sid={value}");
            }
        }

        public Api(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(HostAddress);
            _httpInteractor = new HttpInteractor(httpClient, BaseUrl + $"/{Version}");
            _apiResultBuilder = new ApiResultBuilder();
        }

        // login POST
        public async Task<ApiResult<Account>> LoginAsync(string email, string password)
        {
            var data = new Dictionary<string, object>
            {
                ["email"] = email,
                ["secret"] = $"0 {password}",
                ["deviceID"] = DeviceId,
                ["clientType"] = 100,
                ["action"] = "normal"
            };

            var response = await _httpInteractor.PostAsJsonAsync("/g/s/auth/login", data);

            return _apiResultBuilder.Build<Account>(response);
        }

        public async Task<ApiResult<Account>> LoginByPhonenumberAsync(string phonenumber, string password)
        {
            var data = new Dictionary<string, object>
            {
                ["phoneNumber"] = phonenumber,
                ["secret"] = $"0 {password}",
                ["deviceID"] = DeviceId,
                ["clientType"] = 100,
                ["action"] = "normal"
            };

            var response = await _httpInteractor.PostAsJsonAsync("/g/s/auth/login", data);

            return _apiResultBuilder.Build<Account>(response);
        }

        public async Task<ApiResult<CommunityList>> GetJoinedCommunitiesAsync(int start = 0, int size = 50)
        {
            var response = await _httpInteractor.GetAsync($"/g/s/community/joined?start={start}&size={size}");
            return _apiResultBuilder.Build<CommunityList>(response);
        }

        public async Task<ApiResult<CommunityList>> GetSuggestedCommunitiesAsync(int start = 0, int size = 50)
        {
            var response = await _httpInteractor.GetAsync($"/g/s/community/suggested?start={start}&size={size}");
            return _apiResultBuilder.Build<CommunityList>(response);
        }

        public async Task<ApiResult<CommunityList>> GetCommuntiesByQueryAsync(string query, int start = 0, int size = 0)
        {
            var q = WebUtility.UrlEncode(query);
            var response = await _httpInteractor.GetAsync($"/g/s/community/search?q={q}start={start}&size={size}&language=en");
            return _apiResultBuilder.Build<CommunityList>(response);
        }

        public async Task<ApiResult<UserProfile>> JoinAminoAsync(string id)
        {
            var response = await _httpInteractor.PostAsync($"/x{id}/s/community/join");
            return _apiResultBuilder.Build<UserProfile>(response);
        }

        public async Task<ApiResult<UserProfile>> GetUserProfileByIdAsync(string communityId, string userId)
        {
            var response = await _httpInteractor.GetAsync($"/x{communityId}/s/user-profile/{userId}");
            return _apiResultBuilder.Build<UserProfile>(response);
        }

        public async Task<ApiResult<CommunityCollectionResponse>> GetCommunityCollectionBySectionsAsync(int start = 0, int size = 25, string languageCode = "en")
        {
            var response = await _httpInteractor.GetAsync($"/g/s/community-collection/view/explore/sections?language={languageCode}&start={start}&size={size}");
            return _apiResultBuilder.Build<CommunityCollectionResponse>(response);
        }

        public async Task<ApiResult<BlogList>> GetBlogsByUserIdAsync(string communityId, string userId,
            int start = 0, int size = 25)
        {
            var response = await _httpInteractor.GetAsync(
                $"/x{communityId}/s/blog?type=user&q={userId}&start={start}&size={size}&pagingType=t");
            return _apiResultBuilder.Build<BlogList>(response);
        }

        public async Task<ApiResult<Blog>> PostBlogAsync(string communityId, string title, string content,
            IEnumerable<ImageItem> imageItems = null)
        {
            const int type = 0;

            if (imageItems == null || !imageItems.Any()) imageItems = null;

            var data = new Dictionary<string, object>
            {
                ["title"] = title,
                ["content"] = content,
                ["mediaList"] = null,
                ["timestamp"] = Helpers.GetUnixTimeStamp() + "000",
                ["type"] = type,
                ["latitude"] = 0,
                ["longitude"] = 0,
                ["extensions"] = null,
                ["address"] = null
            };

            if (imageItems != null) AddMediaToData(data, imageItems);

            var result = await _httpInteractor.PostAsJsonAsync($"/x{communityId}/s/blog", data);
            return _apiResultBuilder.Build<Blog>(result);
        }

        private void AddMediaToData(Dictionary<string, object> data, IEnumerable<ImageItem> imageItems)
        {
            var jArray = new JArray();
            foreach (var imageItem in imageItems)
            {
                var innerArray = new JArray();
                innerArray.Add(100);
                innerArray.Add(imageItem.ImageUrl.ToString());
                innerArray.Add(null);
                if (imageItem.BlogReferenceId != null) innerArray.Add(imageItem.BlogReferenceId);
                jArray.Add(innerArray);
            }
            data["mediaList"] = jArray;
        }

        public async Task<ApiResult<FeedHeadlines>> GetFeedHeadlinesAsync(int start = 0, int size = 25)
        {
            var response = await _httpInteractor.GetAsync($"/g/s/feed/headlines?start={start}&size={size}");
            return _apiResultBuilder.Build<FeedHeadlines>(response);
        }

        public async Task<ApiResult<ImageItem>> UploadImageAsync(Stream imageStream)
        {
            var response = await _httpInteractor.PostStreamAsync("/g/s/media/upload", imageStream);
            return _apiResultBuilder.Build<ImageItem>(response);
        }

        public async Task<ApiResult<ThreadList>> GetJoinedChatsAsync(string communityId, int start = 0, int size = 25)
        {
            var response = await _httpInteractor.GetAsync($"/x{communityId}/s/chat/thread?type=joined-me&start={start}&size={size}&cv=1.2");
            return _apiResultBuilder.Build<ThreadList>(response);
        }

        public async Task<ApiResult<ThreadCheckList>> ThreadCheckAsync(string communityId)
        {
            var response = await _httpInteractor.GetAsync($"/x{communityId}/s/chat/thread-check");
            return _apiResultBuilder.Build<ThreadCheckList>(response);
        }

        public async Task<ApiResult<MessageList>> GetMessagesForUserByCommunityIdAsync(string communityId, string threadId, int start = 0, int size = 25)
        {
            var response = await _httpInteractor.GetAsync($"/x{communityId}/s/chat/thread/{threadId}/message?start={start}&size={size}&cv=1.2");
            return _apiResultBuilder.Build<MessageList>(response);
        }

        public async Task<ApiResult<Message>> SendMessageToChatAsync(string communityId, string threadId, string content)
        {
            var data = new Dictionary<string, object>
            {
                ["attachedObject"] = null,
                ["clientRefId"] = "123456789",
                ["content"] = content,
                ["timestamp"] = Helpers.GetUnixTimeStamp() + "000",
                ["type"] = 0
            };
            var response = await _httpInteractor.PostAsJsonAsync($"/x{communityId}/s/chat/thread/{threadId}/message", data);
            return _apiResultBuilder.Build<Message>(response);
        }

        public async Task<ApiResult<Message>> SendImageToChatAsync(string communityId, string threadId,
            string base64JpgImageData)
        {
            var data = new Dictionary<string, object>
            {
                ["attachedObject"] = null,
                ["clientRefId"] = "123456789",
                ["content"] = null,
                ["mediaType"] = 100,
                ["mediaUploadValue"] = base64JpgImageData,
                ["mediaUploadValueContentType"] = "image/jpg",
                ["timestamp"] = Helpers.GetUnixTimeStamp() + "000",
                ["type"] = 0
            };
            var response = await _httpInteractor.PostAsJsonAsync($"/x{communityId}/s/chat/thread/{threadId}/message", data);
            return _apiResultBuilder.Build<Message>(response);
        }

        public async Task<ApiResult<UserProfileList>> GetMembersForCommunityAsync(string communityId, MemberType memberType, int index = 0, int amount = 25)
        {
            var type = "";
            switch (memberType)
            {
                case MemberType.RecentlyJoined:
                    type = "recent";
                    break;
                case MemberType.Curator:
                    type = "curator";
                    break;
                case MemberType.Leader:
                    type = "leader";
                    break;
                case MemberType.Name:
                    type = "name";
                    break;
            }
            var response = await _httpInteractor.GetAsync($"/x{communityId}/s/user-profile?type={type}&start={index}&size={amount}");
            return _apiResultBuilder.Build<UserProfileList>(response);
        }

        public async Task<ApiResult<UserProfile>> PostCommentOnUsersWallForCommunityAsync(string communityId, string uid, string comment)
        {
            var data = new Dictionary<string, object>
            {
                ["content"] = comment,
                ["mediaList"] = new JArray(),
                ["timestamp"] = Helpers.GetUnixTimeStamp() + "000",
            };
            var response = await _httpInteractor.PostAsJsonAsync($"/x{communityId}/s/user-profile/{uid}/comment", data);
            return _apiResultBuilder.Build<UserProfile>(response);
        }

        public async Task<ApiResult<BlogFeed>> GetBlogFeedForCommunityAsync(string communityId, int size = 25, string pageToken = null)
        {
            // &pageToken=ZnwxNTU0NTE0NDYyfGY5MTBlYjYwLWQwOTItNDIxYi1iZmVhLTJiMzY4MmMyMTVlYQ
            var url = $"/x{communityId}/s/feed/blog-all?pagingType=t&size={size}";
            if (pageToken != null)
            {
                url += $"&pageToken={pageToken}";
            }
            var response = await _httpInteractor.GetAsync(url);
            return _apiResultBuilder.Build<BlogFeed>(response);
        }

        public async Task<ApiResult> VoteBlog(string communityId, string blogId, VoteValue value, VoteEventSource source)
        {
            string eventSource = null;
            switch (source)
            {
                case VoteEventSource.FeedList:
                    eventSource = "FeedList";
                    break;
            }

            var data = new Dictionary<string, object>
            {
                ["value"] = (int)value,
                ["eventSource"] = eventSource,
                ["timestamp"] = Helpers.GetUnixTimeStamp() + "000",
            };

            var url = $"/x{communityId}/s/blog/{blogId}/vote?value={(int)value}&eventSource={eventSource}";
            var response = await _httpInteractor.PostAsJsonAsync(url, data);
            return _apiResultBuilder.BuildInfoOnly(response);
        }

        public async Task<ApiResult> FollowMember(string communityId, string memberId)
        {
            var url = $"/x{communityId}/s/user-profile/{memberId}/member";
            var response = await _httpInteractor.PostAsync(url);
            return _apiResultBuilder.BuildInfoOnly(response);
        }

        public async Task<ApiResult> VoteBlogs(string communityId, string[] blogIds, VoteValue value)
        {
            var data = new Dictionary<string, object>
            {
                ["value"] = (int)value,
                ["targetIdList"] = blogIds,
                ["timestamp"] = Helpers.GetUnixTimeStamp() + "000",
            };

            var url = $"x{communityId}/s/feed/vote";
            var response = await _httpInteractor.PostAsJsonAsync(url, data);
            return _apiResultBuilder.BuildInfoOnly(response);
        }

        public async Task<ApiResult> FollowMembers(string communityId, string myUserId, string[] userIds)
        {
            var data = new Dictionary<string, object>
            {
                ["targetUidList"] = userIds,
                ["timestamp"] = Helpers.GetUnixTimeStamp() + "000",
            };

            var url = $"/x{communityId}/s/user-profile/{myUserId}/joined";
            var response = await _httpInteractor.PostAsJsonAsync(url, data);
            return _apiResultBuilder.BuildInfoOnly(response);
        }

        public async Task<ApiResult> RemoveFromChatAsync(string communityId, string threadId, string memberId)
        {
            var url = $"/x{communityId}/s/chat/thread/{threadId}/member/{memberId}";
            var response = await _httpInteractor.DeleteAsync(url);
            return _apiResultBuilder.BuildInfoOnly(response);
        }
    }
}