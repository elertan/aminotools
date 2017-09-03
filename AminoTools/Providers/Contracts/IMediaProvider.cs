using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Media;

namespace AminoTools.Providers.Contracts
{
    public interface IMediaProvider
    {
        Task<ImageItem> UploadImage(Stream imageStream);
    }
}
