using System.Threading.Tasks;
using AminoApi;
using AminoApi.Models.Auth;

namespace AminoTools.Providers.Contracts
{
    public interface IAuthorizationProvider
    {
        Task<ApiResult<Account>> Login(string email, string password);
    }
}
