using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Chat
{
    public class MessageList : ModelBase
    {
        private List<Message> _messages;

        public List<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            var jArray = (JArray) data["messageList"];
            var enumerable = jArray.ToObject<IEnumerable<Dictionary<string, object>>>();

            var messages = new List<Message>();
            foreach (var item in enumerable)
            {
                var message = new Message();
                message.JsonResolve(item);
                messages.Add(message);
            }
            Messages = messages;
        }
    }
}
