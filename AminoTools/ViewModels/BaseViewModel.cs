using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AminoTools.Annotations;
using AminoTools.Models;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class BaseViewModel : IViewModel, INotifyPropertyChanged
    {
        public object ViewModelParameter { get; set; }
        private readonly List<Task> _doAsBusyStateTasks;

        public BaseViewModel()
        {
            IsBusyData = new IsBusyData();
            _doAsBusyStateTasks = new List<Task>();
        }

        public IsBusyData IsBusyData { get; }

        public bool WantsDisposal { get; set; }

        /// <summary>
        /// Is the view model currently initializing
        /// </summary>
        public bool IsInitializing
        {
            get;
            private set;
        }

        /// <summary>
        /// In case the page will disappear, use this to determine wether to cancel any running tasks
        /// </summary>
        public bool IsInitializationCanceled
        {
            get;
            private set;
        }

        public App App => (App) Application.Current;

        public Page Page { get; set; }

        public event EventHandler Initialize;

        public event EventHandler CancelledInitialization;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Executes a task and sets IsBusy to true until all given tasks completed.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected Task DoAsBusyState(Task task)
        {
            HandleDoAsBusyState(task);
            return task;
        }

        /// <summary>
        /// Executes a task and sets IsBusy to true until all given tasks completed.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected Task<T> DoAsBusyState<T>(Task<T> task)
        {
            HandleDoAsBusyState(task);
            return task;
        }

        protected Task DoAsBusyStateCustom(Func<Task> action, CancellationTokenSource cts = null)
        {
            if (cts != null) IsBusyData.SetCancellationTokenSource(cts);
            
            var task = action();
            HandleDoAsBusyState(task, true);
            return task;
        }

        private async void HandleDoAsBusyState(Task task, bool resetIsBusyData = false)
        {
            try
            {
                if (!IsBusyData.IsBusy)
                {
                    IsBusyData.IsBusy = true;
                }
                _doAsBusyStateTasks.Add(task);
                await task;
                if (resetIsBusyData)
                {
                    IsBusyData.Description = null;
                    IsBusyData.Progress = default(float);
                    IsBusyData.IsProgessBarVisible = false;
                    IsBusyData.ClearCancellationTokenSource();
                }
                _doAsBusyStateTasks.Remove(task);
                if (_doAsBusyStateTasks.Any()) return;
                IsBusyData.IsBusy = false;
            }
            catch (Exception ex)
            {
                await Page.DisplayAlert("Error", ex.Message, "Close Application");
                throw ex;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnInitialize()
        {
            App.CurrentViewModel = this;

            IsInitializing = true;
            Initialize?.Invoke(this, EventArgs.Empty);
            IsInitializing = false;
        }

        public virtual void OnCancelledInitialization()
        {
            IsInitializationCanceled = true;
            CancelledInitialization?.Invoke(this, EventArgs.Empty);
        }
    }
}
