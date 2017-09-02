using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models.Feed
{
    public class HeadLineBlog : Blog.Blog
    {
        private string _communityId;
        private Community.Community _community;

        public string CommunityId
        {
            get => _communityId;
            set
            {
                _communityId = value; 
                OnPropertyChanged();
            }
        }

        public Community.Community Community
        {
            get => _community;
            set
            {
                _community = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            base.JsonResolve(data);

            CommunityId = Convert.ToString(data["ndcId"]);
        }
    }
}
