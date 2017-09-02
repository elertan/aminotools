using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Auth;
using AminoTools.Providers.Contracts;
using Xamarin.Forms;

namespace AminoTools.Providers
{
    public class AuthorizationProvider : Provider, IAuthorizationProvider
    {
        public AuthorizationProvider(IApi api) : base(api)
        {
            
        }

        public async Task<Account> Login(string email, string password)
        {
            var result = await Api.Login(email, password);
            return result.Data;
        }
    }
}
