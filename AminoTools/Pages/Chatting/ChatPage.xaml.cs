﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.CustomPages;
using AminoTools.Models.Chatting.GlobalChatting;
using AminoTools.ViewModels.Chatting;
using AminoTools.ViewModels.Contracts.Chatting;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.Pages.Chatting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : BasePage
    {
        public ChatCommunityModel ChatCommunityModel { get; }

        public ChatPage(ChatCommunityModel chatCommunityModel) : base()
        {
            ChatCommunityModel = chatCommunityModel;
            InitializeComponent();
        }

        public ListView GetListView() => ListView;

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            ((ListView) sender).SelectedItem = null;
        }
    }
}