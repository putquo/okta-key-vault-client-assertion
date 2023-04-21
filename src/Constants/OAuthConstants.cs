namespace IdentityBridge.Constants;

public static class OAuthConstants
{
    public static class GrantTypes
    {
        public const string ClientCredentials = "client_credentials";
    }

    public static class ClientAssertionTypes
    {
        public const string JwtBearer = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";
    }
    public static class TokenTypes
    {
        public const string Bearer = "Bearer";
    }
}
