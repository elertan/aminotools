using System;
using System.Collections.Generic;
using AminoApi.Models.Media;
using Newtonsoft.Json.Linq;
using SQLiteNetExtensions.Attributes;

namespace AminoApi.Models.Community
{
    public class Community : ModelBase
    {
        private string _linkUrl;
        private int _amountOfMembers;
        private string _name;
        private string _primaryLanguage;
        private string _id;
        private string _tagline;
        private string _iconUrl;
        private string _splashArtUri;
        private List<ImageItem> _imageItems;

        public string IconUrl
        {
            get => _iconUrl;
            set
            {
                _iconUrl = value; 
                OnPropertyChanged();
            }
        }

        [OneToMany]
        public List<ImageItem> ImageItems
        {
            get => _imageItems;
            set
            {
                _imageItems = value; 
                OnPropertyChanged();
            }
        }

        public string SplashArtUri
        {
            get => _splashArtUri;
            set
            {
                _splashArtUri = value; 
                OnPropertyChanged();
            }
        }

        public string LinkUrl
        {
            get => _linkUrl;
            set
            {
                _linkUrl = value; 
                OnPropertyChanged();
            }
        }

        public int AmountOfMembers
        {
            get => _amountOfMembers;
            set
            {
                _amountOfMembers = value; 
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value; 
                OnPropertyChanged();
            }
        }

        public string PrimaryLanguage
        {
            get => _primaryLanguage;
            set
            {
                _primaryLanguage = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Also known as ndcId, or the id the community is referenced by
        /// </summary>
        public string Id
        {
            get => _id;
            set
            {
                _id = value; 
                OnPropertyChanged();
            }
        }

        public string Tagline
        {
            get => _tagline;
            set
            {
                _tagline = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            Id = data.Resolve<string>("ndcId");
            Name = data.Resolve<string>("name");
            PrimaryLanguage = data.Resolve<string>("primaryLanguage");
            AmountOfMembers = data.Resolve<int>("membersCount");
            var linkStr = data.Resolve<string>("link");
            if (!string.IsNullOrWhiteSpace(linkStr)) LinkUrl = linkStr;
            Tagline = data.Resolve<string>("tagline");

            //var jArray = (JArray) data["launchPage"].ToJObject()["mediaList"];
            //if (jArray == null) return;

            //var items = new List<ImageItem>();
            //foreach (var item in jArray)
            //{
            //    var array = (JArray)item;
            //    var imageItem = new ImageItem();
            //    imageItem.JsonResolveArray(array.ToObject<object[]>());

            //    items.Add(imageItem);
            //}
            //ImageItems = items;

            var iconString = data.Resolve<string>("icon");
            if (!string.IsNullOrWhiteSpace(iconString)) IconUrl = iconString;
        }
    }
}
