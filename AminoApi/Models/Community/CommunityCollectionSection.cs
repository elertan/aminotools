using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models.Community
{
    public class CommunityCollectionSection : ModelBase
    {
        public string CollectionId { get; set; }
        public IEnumerable<Community> Communities { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            CollectionId = data.Resolve<string>("collectionId");

        }
    }
}
