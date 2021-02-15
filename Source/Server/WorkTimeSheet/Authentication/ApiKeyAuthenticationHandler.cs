using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.Excepions;

namespace WorkTimeSheet.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeySchemeOptions>
    {
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeySchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(Constants.ApiKey, out var authToken))
                return AuthenticateResult.NoResult();

            var token = authToken.ToString().TrimStart().TrimEnd();

            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.NoResult();

            var accessToken = await GetAccessTokenByApiKey(token);
            if (accessToken == null)
                return AuthenticateResult.NoResult();

            var userId = accessToken.UserId.ToString();
            var roles = new List<string> { Constants.UserRoleOwner };

            var claims = new List<Claim> { new Claim(ClaimTypes.Name.ToString(), userId.ToString()) };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }

        private Task<AccessToken> GetAccessTokenByApiKey(string apiKey)
        {
            return Task.Run(() =>
            {
                var dbContext = (IDbContext)Request.HttpContext.RequestServices.GetService(typeof(IDbContext));
                return dbContext.AccessTokens.FirstOrDefault(x => x.ApiKey == apiKey);
            });
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            var errorMessage = new ErrorMessage()
            {
                Code = "Error_Unauthorized_access",
                Description = "Unauthorized access",
                Message = "Unauthorized access"
            };
            await Response.WriteAsync(JsonConvert.SerializeObject(errorMessage));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            var errorMessage = new ErrorMessage()
            {
                Code = "Error_Forbidden_Access",
                Description = "Forbidden access",
                Message = "Forbidden access"
            };
            await Response.WriteAsync(JsonConvert.SerializeObject(errorMessage));
        }
    }
}
