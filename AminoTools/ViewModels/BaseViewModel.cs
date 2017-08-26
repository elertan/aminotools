using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AminoTools.Annotations;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private readonly List<Task> _doAsBusyStateTasks;

        public BaseViewModel()
        {
            _doAsBusyStateTasks = new List<Task>();
        }

        public bool IsBusy => _isBusy;

        public App App => (App) Application.Current;

        public Page Page { get; set; }

        public virtual void Initialize()
        {
            
        }

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

        private async void HandleDoAsBusyState(Task task)
        {
            if (!_isBusy)
            {
                _isBusy = true;
                OnPropertyChanged(nameof(IsBusy));
            }
            _doAsBusyStateTasks.Add(task);
            await task;
            _doAsBusyStateTasks.Remove(task);
            if (_doAsBusyStateTasks.Any()) return;

            _isBusy = false;
            OnPropertyChanged(nameof(IsBusy));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
