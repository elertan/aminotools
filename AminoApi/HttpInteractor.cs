using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AminoApi
{
    public class HttpInteractor
    {
        private readonly HttpClient _httpClient;

        public HttpInteractor(HttpClient _httpClient)
        {
            this._httpClient = _httpClient;
        }

        public async Task<string> GetAsync(string url, DecompressionMethods dm = DecompressionMethods.None)
        {
            var response = await _httpClient.GetStringAsync(url);
            switch (dm)
            {
                case DecompressionMethods.None:
                    return response;
                case DecompressionMethods.GZip:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task<string> PostAsJsonAsync(string url, object data, DecompressionMethods dm = DecompressionMethods.None)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json);
            var response = await _httpClient.PostAsync(url, content);

            switch (dm)
            {
                case DecompressionMethods.None:
                    return await response.Content.ReadAsStringAsync();
                case DecompressionMethods.GZip:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
