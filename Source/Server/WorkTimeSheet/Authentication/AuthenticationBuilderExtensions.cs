using Microsoft.AspNetCore.Authentication;
using System;

namespace WorkTimeSheet.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder, Action<ApiKeySchemeOptions> options)
        {
            return authenticationBuilder.AddScheme<ApiKeySchemeOptions, ApiKeyAuthenticationHandler>(ApiKeySchemeOptions.DefaultScheme, options);
        }
    }
}
