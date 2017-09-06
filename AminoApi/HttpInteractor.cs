using System;
using System.Collections.Generic;
using System.IO;
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
        /// <summary>
        /// Amount each api call should wait after issueing one, to prevent amino from fucking up your account.
        /// </summary>
        public int LimitApiCallsDelay { get; set; } = 200;

        private DateTime _lastIssuedApiCall = DateTime.Now.Subtract(TimeSpan.FromDays(1));

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

        private async Task WaitForApiDelay()
        {
            var now = DateTime.Now;
            var readyTime = _lastIssuedApiCall.AddMilliseconds(LimitApiCallsDelay);
            if (readyTime > now)
            {
                // We have to wait until that time happens
                var delay = readyTime - now;
                await Task.Delay(delay.Milliseconds + 1);
                await WaitForApiDelay();
            }

            _lastIssuedApiCall = DateTime.Now;
        }

        public async Task<string> GetAsync(string url, DecompressionMethods dm = DecompressionMethods.None)
        {
            await WaitForApiDelay();

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

        public async Task<string> PostAsync(string url, DecompressionMethods dm = DecompressionMethods.None)
        {
            await WaitForApiDelay();

            var response = await _httpClient.PostAsync(_prefix + url, new StringContent(String.Empty));
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

        public async Task<string> PostAsJsonAsync(string url, Dictionary<string, object> data, DecompressionMethods dm = DecompressionMethods.None)
        {
            await WaitForApiDelay();

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

        public async Task<string> PostStreamAsync(string url, Stream stream, DecompressionMethods dm = DecompressionMethods.None)
        {
            await WaitForApiDelay();

            var content = new StreamContent(stream);
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
