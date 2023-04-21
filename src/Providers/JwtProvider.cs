using IdentityBridge.Services;
using static IdentityBridge.Constants.OAuthConstants;

namespace IdentityBridge.Providers;

public interface IJwtProvider
{
  Task<string> GetAuthorizationHeaderAsync();
}

public class DefaultJwtProvider : IJwtProvider
{
  private readonly IOAuthService _client;
  private string _jwt;
  private DateTime _expires;

  public DefaultJwtProvider(IOAuthService client)
  {
    _client = client;
    _jwt = string.Empty;
  }

  public async Task<string> GetAuthorizationHeaderAsync()
  {
    if (string.IsNullOrEmpty(_jwt) || _expires < DateTime.Now)
    {
      var response = await _client.GetBearerTokenAsync();
      _jwt = response.AccessToken;
      _expires = DateTime.Now.AddSeconds(response.ExpiresIn - 1000);
    }
    return $"{TokenTypes.Bearer} {_jwt}";
  }
}
