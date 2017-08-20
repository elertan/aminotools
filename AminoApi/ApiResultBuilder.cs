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
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            var info = new ApiResultInfo();
            info.JsonResolve(data);
            var instance = (ApiModel)Activator.CreateInstance<T>();
            instance.JsonResolve(data);

            return new ApiResult<T> {Data = (T)instance, Info = info};
        }
    }
}
