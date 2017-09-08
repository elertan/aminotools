using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Media;
using AminoTools.Providers.Contracts;

namespace AminoTools.Providers
{
    public class MediaProvider : Provider, IMediaProvider
    {
        public MediaProvider(IApi api) : base(api)
        {
        }

        public async Task<ApiResult<ImageItem>> UploadImage(Stream imageStream)
        {
            var result = await Api.UploadImageAsync(imageStream);
            return result;
        }
    }
}
