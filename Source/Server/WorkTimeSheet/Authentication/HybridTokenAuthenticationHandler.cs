using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.Excepions;

namespace WorkTimeSheet.Authentication
{
    public class HybridTokenAuthenticationHandler : AuthenticationHandler<HybridTokenAuthenticationSchemeOptions>
    {
        public HybridTokenAuthenticationHandler(IOptionsMonitor<HybridTokenAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(Constants.Authorization, out var authToken))
                return AuthenticateResult.NoResult();

            var token = authToken.ToString();
            if (token.StartsWith(Constants.Bearer, StringComparison.InvariantCultureIgnoreCase))
                token = token.Replace(Constants.Bearer, string.Empty);
            token = token.TrimStart().TrimEnd();

            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.NoResult();

            bool isValidToken = false;
            int userId = int.MinValue;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var pricipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Constants.JwtTokenKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                }, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    isValidToken = true;
                    userId = string.IsNullOrEmpty(pricipal.Identity.Name) ? int.MinValue : int.Parse(pricipal.Identity.Name);
                }
            }
            catch (Exception)
            {
                var accessToken = await GetAccessTokenByApiKey(token);
                if (accessToken != null)
                {
                    isValidToken = true;
                    userId = accessToken.UserId;
                }
            }

            if (!isValidToken || userId == int.MinValue)
            {
                return AuthenticateResult.NoResult();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name.ToString(), userId.ToString()) };
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
