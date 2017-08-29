using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace AminoTools
{
    public class ViewModelResolver
    {
        private Type _viewModelType;

        public Type ViewModelType
        {
            get => _viewModelType;
            set
            {
                _viewModelType = value;
                if (value == null) return;
                Model = Resolve(value);
            }
        }

        public IViewModel Model { get; protected set; }

        public IViewModel Resolve(Type viewModelType)
        {
            var app = (App) Application.Current;
            
            var viewModel = (IViewModel)app.DependencyManager.Container.Resolve(viewModelType);
            return viewModel;
        }
    }
}
