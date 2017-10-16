using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;

namespace AminoTools.Providers
{
    public class ApiProvider
    {
        protected readonly IApi Api;

        public ApiProvider(IApi api)
        {
            Api = api;
        }
    }
}
