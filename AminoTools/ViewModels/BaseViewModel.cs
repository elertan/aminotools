using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AminoTools.Annotations;
using AminoTools.Models;
using Xamarin.Forms;

namespace AminoTools.ViewModels
{
    public class BaseViewModel : IViewModel, INotifyPropertyChanged
    {
        private readonly List<Task> _doAsBusyStateTasks;

        public BaseViewModel()
        {
            IsBusyData = new IsBusyData();
            _doAsBusyStateTasks = new List<Task>();
        }

        public IsBusyData IsBusyData { get; }


        public bool IsInitializing
        {
            get;
            private set;
        }

        public App App => (App) Application.Current;

        public Page Page { get; set; }

        public event EventHandler Initialize;

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

        protected Task DoAsBusyStateCustom(Action action)
        {
            var task = new Task(action);
            task.Start();
            DoAsBusyState(task);
            return task;
        }

        private async void HandleDoAsBusyState(Task task)
        {
            if (!IsBusyData.IsBusy)
            {
                IsBusyData.IsBusy = true;
            }
            _doAsBusyStateTasks.Add(task);
            await task;
            _doAsBusyStateTasks.Remove(task);
            if (_doAsBusyStateTasks.Any()) return;
            IsBusyData.IsBusy = false;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnInitialize()
        {
            IsInitializing = true;
            Initialize?.Invoke(this, EventArgs.Empty);
            IsInitializing = false;
        }
    }
}
