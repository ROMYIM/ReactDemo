using System.Collections.Generic;

namespace ReactDemo.Infrastructure.Config.Authentication
{
    public class JwtConfig
    {
        public string SchemeName { get; set; }

        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }

    public class DefaultConfig
    {
        public string SchemeName { get; set; }

        public string SecretKey { get; set; }

        public string CookieName { get; set; }

        public string TokenName { get; set; }

        public string LoginPath { get; set; }

        public string LogoutPath { get; set; }

        public List<string> WhiteList { get; set; }
    }

    public class AuthenticationConfig
    {
        public JwtConfig Jwt { get; set; }

        public DefaultConfig Default { get; set; }
    }
}