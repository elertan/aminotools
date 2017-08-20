using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AminoTools.Annotations;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value; 
                OnPropertyChanged();
            }
        }

        public App App => (App) Application.Current;

        public void Initialize()
        {
            OnReadyForInitialization();
        }

        public event EventHandler ReadyForInitialization;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnReadyForInitialization()
        {
            ReadyForInitialization?.Invoke(this, EventArgs.Empty);
        }
    }
}
