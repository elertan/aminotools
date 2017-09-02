using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoTools.Providers.Contracts;

namespace AminoTools.Providers
{
    public class MediaProvider : Provider, IMediaProvider
    {
        public MediaProvider(IApi api) : base(api)
        {
        }
    }
}
