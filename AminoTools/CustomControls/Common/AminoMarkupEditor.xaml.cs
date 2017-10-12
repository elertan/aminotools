using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Models.Common.AminoMarkupEditor;
using MvvmHelpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AminoTools.CustomControls.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AminoMarkupEditor : ContentView
    {
        public Page Page { get; set; }

        private ObservableRangeCollection<MarkupBlock> _markupBlocks;
        private string _stringRepresentation;

        public ObservableRangeCollection<MarkupBlock> MarkupBlocks
        {
            get => _markupBlocks;
            set
            {
                _markupBlocks = value; 
                OnPropertyChanged();
            }
        }

        public string StringRepresentation
        {
            get => _stringRepresentation;
            set
            {
                _stringRepresentation = value; 
                OnPropertyChanged();
            }
        }

        public Command AddMarkupBlockCommand { get; }

        public AminoMarkupEditor()
        {
            MarkupBlocks = new ObservableRangeCollection<MarkupBlock>
            {
                new MarkupBlock {Text = "Test!"},
                new MarkupBlock {Text = "Test2!"},
            };
            AddMarkupBlockCommand = new Command(DoAddMarkupBlock);

            InitializeComponent();
        }

        private async void DoAddMarkupBlock()
        {
            const string imageOption = "Image";
            const string paragraphOption = "Paragraph";

            var result = await Page.DisplayActionSheet("Select MarkupBlock Type", "Cancel", null, imageOption, paragraphOption);
            if (string.IsNullOrWhiteSpace(result)) return;

            switch (result)
            {
                case imageOption:
                    await Page.DisplayAlert("Not Supported", "Images are not yet supported", "Ok");
                    break;
                case paragraphOption:
                    MarkupBlocks.Add(new MarkupBlock());
                    break;
            }
        }

        private void Editor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var editor = (Editor) sender;
            var lines = editor.Text.Split('\n').Length;
            if (lines == 0) lines++;
            var newHeight = 21 + (editor.FontSize + 4.5) * lines;
            if (Math.Abs(editor.Height - newHeight) > 1) editor.HeightRequest = newHeight;
        }
    }
}