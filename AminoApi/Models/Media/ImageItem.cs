using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models.Media
{
    public class ImageItem : ModelBase
    {
        private int _someValue;
        private Uri _imageUri;
        private string _blogReferenceId;

        public int SomeValue
        {
            get => _someValue;
            set
            {
                _someValue = value; 
                OnPropertyChanged();
            }
        }

        public Uri ImageUri
        {
            get => _imageUri;
            set
            {
                _imageUri = value;
                OnPropertyChanged();
            }
        }

        public string BlogReferenceId
        {
            get => _blogReferenceId;
            set
            {
                _blogReferenceId = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolveArray(object[] data)
        {
            SomeValue = Convert.ToInt32(data[0]);
            ImageUri = new Uri(Convert.ToString(data[1]));
            if (DoesContainBlogReferenceId(data))
            {
                BlogReferenceId = Convert.ToString(data[3]);
            }
        }

        private bool DoesContainBlogReferenceId(object[] data)
        {
            return data.Length > 3;
        }
    }
}
