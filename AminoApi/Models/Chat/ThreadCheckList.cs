using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Chat
{
    public class ThreadCheckList : ModelBase
    {
        private List<ThreadCheck> _threadChecks;

        public List<ThreadCheck> ThreadChecks
        {
            get => _threadChecks;
            set
            {
                _threadChecks = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            var jArray = (JArray)data["threadCheckResult"];
            var enumerable = jArray.ToObject<IEnumerable<object[]>>();

            var threadChecks = new List<ThreadCheck>();

            foreach (var d in enumerable)
            {
                var threadCheck = new ThreadCheck();
                threadCheck.JsonResolveArray(d);
                threadChecks.Add(threadCheck);
            }

            ThreadChecks = threadChecks;
        }
    }
}
