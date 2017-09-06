using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Blog;
using AminoApi.Models.Misc;

namespace AminoApi
{
    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }

    public class ApiResult
    {
        public ApiResultInfo Info { get; set; }

        public bool DidSucceed()
        {
            if (Info.Message != "OK"
                || Info.StatusCode != 0) return false;
            return true;
        }

        public static ApiResult<T> Create<T>(T data)
        {
            return new ApiResult<T> { Data = data };
        }

        public static ApiResult<T> Create<T>(T data, ApiResultInfo info)
        {
            var result = Create(data);
            result.Info = info;
            return result;
        }

        public static ApiResult CreateWithInfoOnly(ApiResultInfo info)
        {
            return new ApiResult {Info = info};
        }
    }
}
