using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;

namespace AminoTools.Providers
{
    public class Provider
    {
        protected readonly Api Api;

        public Provider(Api api)
        {
            Api = api;
        }
    }
}
