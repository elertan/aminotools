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
        public ApiResult<T> Build<T>(string json) where T : ModelBase
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            var info = new ApiResultInfo();
            info.JsonResolve(data);

            var result = new ApiResult<T> { Info = info };
            if (!result.DidSucceed()) return result;

            var instance = (ModelBase)Activator.CreateInstance<T>();
            instance.JsonResolve(data);
            result.Data = (T)instance;

            return result;
        }

        public ApiResult BuildInfoOnly(string json)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            var info = new ApiResultInfo();
            info.JsonResolve(data);

            var result = new ApiResult { Info = info };

            return result;
        }
    }
}
