using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AminoTools.Models.Chatting.GlobalChatting;
using AminoTools.Pages.Chatting;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.CustomControls.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatCell : ViewCell, INotifyPropertyChanged
    {
        private bool _firstTimeAppearing = true;
        private ChatCommunityModel _chatCommunityModel;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _glowTask;
        private static readonly Color GlowColor = Color.FromHex("#ffd668");
        private readonly Color _defaultColor;
        private static readonly Color BaseHighlightColor = Color.FromHex("#fffecc");


        public ChatCell()
        {
            InitializeComponent();
            Appearing += ChatCell_Appearing;
            
            OpenChatCommand = new Command(DoOpenChat);

            _defaultColor = MainGrid.BackgroundColor;
        }

        private async void DoOpenChat()
        {
            //await ChatImage.RotateTo(-90);
            //await ChatImage.TranslateTo(MainGrid.Width + ChatImage.Width, 0, 500, Easing.CubicIn);
            //await ChatImage.ScaleTo(30, 500, Easing.CubicIn);

            await ((App) Application.Current).MainNavigation.PushAsync(new ChatPage((ChatCommunityModel) BindingContext));

            //ChatImage.Scale = 1;
        }

        public Command OpenChatCommand { get; }

        private void ChatCell_Appearing(object sender, EventArgs e)
        {
            if (!_firstTimeAppearing) return;
            _firstTimeAppearing = false;

            _chatCommunityModel = (ChatCommunityModel) BindingContext;
            _chatCommunityModel.PropertyChanged += Binding_PropertyChanged;

            WireUpEvents();
            if (_chatCommunityModel.Chat.HasUnreadContent) SetupGlowTask();
        }

        private void WireUpEvents()
        {
            _chatCommunityModel.Chat.PropertyChanged += Chat_PropertyChanged;
        }

        private void Chat_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_chatCommunityModel.Chat.HasUnreadContent))
            {
                if (_chatCommunityModel.Chat.HasUnreadContent)
                {
                    SetupGlowTask();
                }
                else
                {
                    _cancellationTokenSource?.Cancel();
                    MainGrid.BackgroundColor = _defaultColor;
                }
            }
        }

        private void SetupGlowTask()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _glowTask = new Task(GlowMethod);
            _glowTask.Start();
        }

        private async void GlowMethod()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                await MainGrid.ColorTo(BaseHighlightColor, GlowColor, c => MainGrid.BackgroundColor = c, 1500U, Easing.SinOut);
                await MainGrid.ColorTo(GlowColor, BaseHighlightColor, c => MainGrid.BackgroundColor = c, 1500U, Easing.SinIn);
            }
        }

        private void Binding_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_chatCommunityModel.Chat))
            {
                _chatCommunityModel.Chat.PropertyChanged += Chat_PropertyChanged;
            }
        }
    }
}