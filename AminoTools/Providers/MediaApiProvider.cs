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
    public class MediaApiProvider : ApiProvider, IMediaProvider
    {
        private readonly IDatabaseProvider _databaseProvider;

        public MediaApiProvider(IApi api,
            IDatabaseProvider databaseProvider) : base(api)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task<ApiResult<ImageItem>> UploadImage(Stream imageStream)
        {
            var result = await Api.UploadImageAsync(imageStream);
            return result;
        }

        public async Task StoreImageItemsAsync(List<ImageItem> imageItems)
        {
            var db = await _databaseProvider.GetDatabaseAsync();
            //var itemsToBeStored = new List<ImageItem>();
            //if (db.Connection.Table<ImageItem>().Where(item => item.))
            await db.Connection.InsertOrIgnoreAllAsync(imageItems);
        }
    }
}
