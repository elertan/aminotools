using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi
{
    public interface IModelResolvable
    {
        void JsonResolve(Dictionary<string, object> data);
    }
}
