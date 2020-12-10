using System.Threading.Tasks;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthorizedUser> Authenticate(string email, string password);

        Task<AuthorizedUser> RefreshAuthentication(AuthorizedUser authorizedUser);
    }
}
