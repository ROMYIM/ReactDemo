using System.Collections.Generic;

namespace ReactDemo.Infrastructure.Config.Authentication
{
    public class JwtOptions
    {
        public string SchemeName { get; set; }

        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }

    public class DefaultOptions
    {
        public string SchemeName { get; set; }

        public string SecretKey { get; set; }

        public string CookieName { get; set; }

        public string TokenName { get; set; }

        public string LoginPath { get; set; }

        public string LogoutPath { get; set; }

        public List<string> WhiteList { get; set; }
    }

    public class AuthenticationOptions
    {
        public JwtOptions Jwt { get; set; }

        public DefaultOptions Default { get; set; }
    }
}