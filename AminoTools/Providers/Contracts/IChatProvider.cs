using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Chat;
using AminoApi.Models.Community;

namespace AminoTools.Providers.Contracts
{
    public interface IChatProvider
    {
        Task<ApiResult<List<Chat>>> GetChatsByCommunityAsync(Community community);
        Task<ApiResult<List<ThreadCheck>>> CheckForNewMessagesAsync(string communityId);
        Task<ApiResult<MessageList>> GetMessagesAsync(string communityId, string threadId, int index = 0);
        Task<ApiResult<Message>> SendMessageToChatAsync(string communityId, string threadId, string content);
        Task<ApiResult<Message>> SendImageToChatAsync(string communityId, string threadId, string base64JpgImageContent);
        Task<List<Chat>> GetStoredChatsAsync();
        Task StoreChatsAsync(List<Chat> chats);
    }
}
