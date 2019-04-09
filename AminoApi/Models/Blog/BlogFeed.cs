using AminoApi.Models.Misc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models.Blog
{
    public class BlogFeed : BlogList
    {
        public Paging Paging { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            base.JsonResolve(data);

            Paging = new Paging();
            var obj = (JObject)data["paging"];
            Paging.JsonResolve(obj.ToObject<Dictionary<string, object>>());
        }
    }
}
