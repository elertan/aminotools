using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models.Media
{
    public class ImageItem : ModelBase
    {
        private int _mediaItemId;
        private string _imageUrl;
        private string _blogReferenceId;
        private string _description;

        public int MediaItemId
        {
            get => _mediaItemId;
            set
            {
                _mediaItemId = value; 
                OnPropertyChanged();
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
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

        public string BlogReferenceId
        {
            get => _blogReferenceId;
            set
            {
                _blogReferenceId = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            var url = data.Resolve<string>("mediaValue");
            if (url != null)
                ImageUrl = url;
        }

        public override void JsonResolveArray(object[] data)
        {
            MediaItemId = Convert.ToInt32(data[0]);
            ImageUrl = Convert.ToString(data[1]);
            Description = Convert.ToString(data[2]);
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
