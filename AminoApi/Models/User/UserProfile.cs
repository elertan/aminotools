using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace AminoApi.Models.User
{
    public class UserProfile : ModelBase
    {
        public int Status { get; set; }
        [PrimaryKey]
        public string Uid { get; set; }
        public string Nickname { get; set; }
        public string IconUrl { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            if (data.ContainsKey("userProfile"))
            {
                JsonResolve(data["userProfile"].ToJObject().ToObject<Dictionary<string, object>>());
                return;
            }

            Status = data.Resolve<int>("status");
            Uid = data.Resolve<string>("uid");
            Nickname = data.Resolve<string>("nickname");

            var iconString = data.Resolve<string>("icon");
            if (iconString != null)
                IconUrl = iconString;
        }
    }
}
