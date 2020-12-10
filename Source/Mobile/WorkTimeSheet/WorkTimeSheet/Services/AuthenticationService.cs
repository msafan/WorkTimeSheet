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
            var jsonSerializer = new JsonSerializer();
            return jsonSerializer.Deserialize<AuthorizedUser>(new JsonTextReader(new StringReader(response)));
        }

        public async Task<AuthorizedUser> RefreshAuthentication(AuthorizedUser authorizedUser)
        {
            var response = await WebApiLayer.PostAsync("authenticate/refresh", authorizedUser);
            var jsonSerializer = new JsonSerializer();
            return jsonSerializer.Deserialize<AuthorizedUser>(new JsonTextReader(new StringReader(response)));
        }
    }
}
