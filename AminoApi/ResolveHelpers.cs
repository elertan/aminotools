using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi
{
    public static class ResolveHelpers
    {
        public static T Resolve<T>(this Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return (T) Convert.ChangeType(dict[key], typeof(T));
            }
            return default(T);
        }
    }
}
