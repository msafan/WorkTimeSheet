using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using WorkTimeSheet.Interfaces;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Services
{
    public class AuthenticationService : WebApiServiceBase, IAuthenticationService
    {
        public AuthenticationService(IWebApiLayer webApiLayer) : base(webApiLayer)
        {

        }

        public async Task<AuthorizedUser> Authenticate(string email, string password)
        {
            var response = await WebApiLayer.PostAsync("authenticate", new { email, password });
            return JsonConvert.DeserializeObject<AuthorizedUser>(response);
        }

        public async Task<AuthorizedUser> RefreshAuthentication(AuthorizedUser authorizedUser)
        {
            var response = await WebApiLayer.PostAsync("authenticate/refresh", authorizedUser);
            return JsonConvert.DeserializeObject<AuthorizedUser>(response);
        }
    }
}
