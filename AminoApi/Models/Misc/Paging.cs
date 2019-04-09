using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models.Misc
{
    public class Paging : ModelBase
    {
        public string NextPageToken { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            NextPageToken = Convert.ToString(data["nextPageToken"]);
        }
    }
}
