using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;
using AminoApi.Models.Auth;
using AminoApi.Models.Blog;
using AminoApi.Models.Community;
using AminoApi.Models.Feed;
using AminoApi.Models.Media;

namespace AminoApi
{
    public class Api
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
        public async Task<ApiResult<Account>> Login(string email, string password)
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

        public async Task<ApiResult<CommunityList>> GetJoinedCommunities(int start = 0, int size = 50)
        {
            var response = await _httpInteractor.GetAsync($"/g/s/community/joined?start={start}&size={size}");
            return _apiResultBuilder.Build<CommunityList>(response);
        }

        public async Task<ApiResult<BlogList>> GetBlogsByUserIdAsync(string communityId, string userId,
            int start = 0, int size = 25)
        {
            var response = await _httpInteractor.GetAsync(
                $"/{communityId}/s/blog?type=user&q={userId}&start={start}&size={size}");
            return _apiResultBuilder.Build<BlogList>(response);
        }

        public async Task<ApiResult<Blog>> PostBlog(string communityId, string title, string content, IEnumerable<ImageItem> imageItems = null)
        {
            const int type = 0;

            if (imageItems == null || !imageItems.Any()) imageItems = null;

            var data = new Dictionary<string, object>
            {
                ["title"] = title,
                ["content"] = content,
                ["mediaList"] = imageItems?.ToArray(),
                ["timestamp"] = Helpers.GetUnixTimeStamp() + "000",
                ["type"] = type,
                ["latitude"] = 0,
                ["longitude"] = 0,
                ["extensions"] = null,
                ["address"] = null
            };

            var result = await _httpInteractor.PostAsJsonAsync($"/x{communityId}/s/blog", data);
            return _apiResultBuilder.Build<Blog>(result);
        }

        public async Task<ApiResult<FeedHeadlines>> GetFeedHeadlines(int start = 0, int size = 25)
        {
            var response = await _httpInteractor.GetAsync($"/g/s/feed/headlines?start={start}&size={size}");
            try
            {
                return _apiResultBuilder.Build<FeedHeadlines>(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}