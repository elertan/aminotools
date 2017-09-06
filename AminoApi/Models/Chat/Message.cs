using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models.Chat
{
    public class Message : ModelBase
    {
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            Content = data.Resolve<string>("content");
            CreatedTime = data.Resolve<DateTime>("createdTime");
            Id = data.Resolve<string>("messageId");
            UserId = data.Resolve<string>("uid");
        }
    }
}
