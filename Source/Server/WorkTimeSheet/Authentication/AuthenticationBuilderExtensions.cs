using Microsoft.AspNetCore.Authentication;
using System;

namespace WorkTimeSheet.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder, Action<HybridTokenAuthenticationSchemeOptions> options)
        {
            return authenticationBuilder.AddScheme<HybridTokenAuthenticationSchemeOptions, HybridTokenAuthenticationHandler>(HybridTokenAuthenticationSchemeOptions.DefaultScheme, options);
        }
    }
}
