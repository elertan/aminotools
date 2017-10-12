using System;
using System.Threading;
using Xamarin.Forms;

namespace AminoTools.Models
{
    public class IsBusyData : BaseModel
    {
        private string _description;
        private float _progress;
        private bool _isBusy;
        private bool _isProgessBarVisible;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isCancelButtonEnabled;
        private bool _isCancellable;

        public IsBusyData()
        {
            IsProgessBarVisible = false;
            IsCancelButtonEnabled = true;
            CancelCommand = new Command(DoCancel);
        }

        public event EventHandler CancelRequested;

        private void DoCancel()
        {
            IsCancelButtonEnabled = false;
            _cancellationTokenSource?.Cancel();
            Description = "Canceling...";
            OnCancelRequested();
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value; 
                OnPropertyChanged();
            }
        }

        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value; 
                OnPropertyChanged();
            }
        }

        public bool IsProgessBarVisible
        {
            get => _isProgessBarVisible;
            set
            {
                _isProgessBarVisible = value; 
                OnPropertyChanged();
            }
        }

        public bool IsCancelButtonEnabled
        {
            get => _isCancelButtonEnabled;
            protected set
            {
                _isCancelButtonEnabled = value; 
                OnPropertyChanged();
            }
        }

        public bool IsCancellable
        {
            get => _isCancellable;
            protected set
            {
                _isCancellable = value;
                OnCancelRequested();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value; 
                OnPropertyChanged();
            }
        }

        public Command CancelCommand { get; }

        public void SetCancellationTokenSource(CancellationTokenSource cts)
        {
            IsCancellable = true;
            _cancellationTokenSource = cts;
        }

        public void ClearCancellationTokenSource()
        {
            IsCancellable = false;
        }

        protected virtual void OnCancelRequested()
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
