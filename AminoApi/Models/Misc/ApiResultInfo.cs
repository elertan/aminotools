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

        public override void Resolve(dynamic data)
        {
            StatusCode = data["api:statuscode"];
            Message = data["api:message"];
            Duration = data["api:duration"];
            TimeStamp = data["api:timestamp"];
        }
    }
}
