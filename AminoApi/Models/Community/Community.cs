using System;
using System.Collections.Generic;

namespace AminoApi.Models.Community
{
    public class Community : ModelBase
    {
        private Uri _link;
        private int _amountOfMembers;
        private string _name;
        private string _primaryLanguage;
        private string _id;
        private string _tagline;
        private Uri _icon;
        private Uri _splashArt;

        public Uri Icon
        {
            get => _icon;
            set
            {
                _icon = value; 
                OnPropertyChanged();
            }
        }

        public Uri SplashArt
        {
            get => _splashArt;
            set
            {
                _splashArt = value; 
                OnPropertyChanged();
            }
        }

        public Uri Link
        {
            get => _link;
            set
            {
                _link = value; 
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
            if (!string.IsNullOrWhiteSpace(linkStr)) Link = new Uri(linkStr);
            Tagline = data.Resolve<string>("tagline");
        }
    }
}
