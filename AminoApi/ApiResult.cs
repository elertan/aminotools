using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models.Misc;

namespace AminoApi
{
    public class ApiResult<T>
    {
        public T Data { get; set; }
        public ApiResultInfo Info { get; set; }
    }
}
