using Microsoft.AspNetCore.Authentication;

namespace WorkTimeSheet.Authentication
{
    public class ApiKeySchemeOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKey";

        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
