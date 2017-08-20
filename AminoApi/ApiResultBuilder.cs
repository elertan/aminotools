using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;
using AminoApi.Models.Misc;
using Newtonsoft.Json;

namespace AminoApi
{
    public class ApiResultBuilder
    {
        private readonly ModelResolver _modelResolver;

        public ApiResultBuilder()
        {
            _modelResolver = new ModelResolver();
        }
        public ApiResult<T> Build<T>(string json) where T : ApiModel
        {
            var o = (dynamic)JsonConvert.DeserializeObject(json);

            var info = new ApiResultInfo();
            info.Resolve(o);
            var instance = (ApiModel)Activator.CreateInstance<T>();
            instance.Resolve(o);

            return new ApiResult<T> {Data = (T)instance, Info = info};
        }
    }
}
