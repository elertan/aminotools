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
        private readonly string _prefix;

        public HttpInteractor(HttpClient httpClient, string prefix = null)
        {
            this._httpClient = httpClient;
            //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            _prefix = prefix;
        }

        public void AddDefaultRequestHeader(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        public void RemoveDefaultRequestHeader(string key)
        {
            _httpClient.DefaultRequestHeaders.Remove(key);
        }

        public async Task<string> GetAsync(string url, DecompressionMethods dm = DecompressionMethods.None)
        {
            var response = await _httpClient.GetStringAsync(_prefix + url);
            switch (dm)
            {
                case DecompressionMethods.None:
                    return response;
                case DecompressionMethods.GZip:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task<string> PostAsJsonAsync(string url, Dictionary<string, object> data, DecompressionMethods dm = DecompressionMethods.None)
        {
            var json = JsonConvert.SerializeObject(data);

            var content = new StringContent(json);
            var response = await _httpClient.PostAsync(_prefix + url, content);

            switch (dm)
            {
                case DecompressionMethods.None:
                    return await response.Content.ReadAsStringAsync();
                case DecompressionMethods.GZip:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
