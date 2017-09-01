using System.Threading.Tasks;
using AminoApi.Models.Auth;

namespace AminoTools.Providers.Contracts
{
    public interface IAuthorizationProvider
    {
        Task<Account> Login(string email, string password);
    }
}
