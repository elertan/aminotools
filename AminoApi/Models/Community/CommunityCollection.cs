using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models.Community
{
    public class CommunityCollection : ModelBase
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            Id = data.Resolve<string>("collectionId");
            Label = data.Resolve<string>("label");
        }
    }
}
