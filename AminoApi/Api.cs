using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;

namespace AminoApi
{
    public class Api
    {
        private readonly HttpInteractor _httpInteractor;
        private readonly ApiResultBuilder _apiResultBuilder;
        private const string _hostAddress = "https://service.narvii.com";
        private const string _baseUrl = "/api";
        public string Version { get; set; } = "v1";

        public string DeviceId { get; set; } =
            "01D5ED5BE317F883719728B66E5D9D7A21BF3596050F95AAB9BC707D2D82AF6F82DFDEF9B1CF305A90";

        public Api(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(_hostAddress);
            _httpInteractor = new HttpInteractor(httpClient, _baseUrl + $"/{Version}");
            _apiResultBuilder = new ApiResultBuilder();

            Global = new GlobalClass(this);
        }

        public GlobalClass Global { get; }
        // g
        public class GlobalClass
        {
            private readonly Api _api;

            public GlobalClass(Api api)
            {
                _api = api;
                S = new SClass(_api);
            }
            public SClass S { get; }
            // s
            public class SClass
            {
                private readonly Api _api;

                public SClass(Api api)
                {
                    _api = api;
                    Auth = new AuthClass(_api);
                }
                public AuthClass Auth { get; }
                // auth
                public class AuthClass
                {
                    private readonly Api _api;

                    public AuthClass(Api api)
                    {
                        _api = api;
                    }
                    // login POST
                    public async Task<ApiResult<Account>> Login(string email, string password)
                    {
                        var data = new Dictionary<string, object>
                        {
                            ["email"] = email,
                            ["secret"] = $"0 {password}",
                            ["deviceID"] = _api.DeviceId,
                            ["clientType"] = 100,
                            ["action"] = "normal"
                        };

                        var response = await _api._httpInteractor.PostAsJsonAsync("/g/s/auth/login", data);

                        return _api._apiResultBuilder.Build<Account>(response);
                    }
                }
            }
        }
    }
}
