using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models.Misc
{
    public class ApiResultInfo : ModelBase
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Duration { get; set; }
        public string TimeStamp { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            StatusCode = Convert.ToInt32(data["api:statuscode"]);
            Message = Convert.ToString(data["api:message"]);
            Duration = Convert.ToString(data["api:duration"]);
            TimeStamp = Convert.ToString(data["api:timestamp"]);
        }
    }
}
