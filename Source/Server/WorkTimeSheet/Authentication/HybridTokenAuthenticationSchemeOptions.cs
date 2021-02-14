using Microsoft.AspNetCore.Authentication;

namespace WorkTimeSheet.Authentication
{
    public class HybridTokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Bearer";

        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
