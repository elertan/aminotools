using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Chat
{
    public class ThreadList : ModelBase
    {
        private List<Chat> _chats;

        public List<Chat> Chats
        {
            get => _chats;
            set
            {
                _chats = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            var jArray = (JArray) data["threadList"];
            var enumerable = jArray.ToObject<IEnumerable<Dictionary<string, object>>>();

            var chats = new List<Chat>();

            foreach (var dictionary in enumerable)
            {
                var chat = new Chat();
                chat.JsonResolve(dictionary);
                chats.Add(chat);
            }

            Chats = chats;
        }
    }
}
