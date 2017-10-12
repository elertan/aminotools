using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoTools.Models.Common.AminoMarkupEditor
{
    public class MarkupBlock : BaseModel
    {
        private Uri _imageUri;
        private string _text;
        private bool _isBold;
        private bool _isItalic;
        private bool _isCentered;

        public Uri ImageUri
        {
            get => _imageUri;
            set
            {
                _imageUri = value; 
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value; 
                OnPropertyChanged();
            }
        }

        public bool IsBold
        {
            get => _isBold;
            set
            {
                _isBold = value; 
                OnPropertyChanged();
            }
        }

        public bool IsItalic
        {
            get => _isItalic;
            set
            {
                _isItalic = value; 
                OnPropertyChanged();
            }
        }

        public bool IsCentered
        {
            get => _isCentered;
            set
            {
                _isCentered = value; 
                OnPropertyChanged();
            }
        }

        public bool IsEntryVisible => ImageUri == null;
        public bool IsImageVisible => ImageUri != null;
    }
}
