using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Chat;
using AminoTools.Models.Chatting.GlobalChatting;
using MvvmHelpers;

namespace AminoTools.ViewModels.Contracts.Chatting
{
    public interface IGlobalChattingPageViewModel
    {
        ObservableRangeCollection<ChatCommunityModel> Chats { get; set; }
        bool IsLoading { get; set; }
        string LoadingText { get; set; }
        float LoadingProgress { get; set; }
    }
}
