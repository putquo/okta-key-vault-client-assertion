using IdentityBridge.Providers;
using RestSharp;
using RestSharp.Authenticators;

namespace IdentityBridge.Authenticators;

public class OktaAuthenticator : AuthenticatorBase
{
  private readonly IJwtProvider _jwtProvider;
  public OktaAuthenticator(IJwtProvider jwtProvider) : base(string.Empty)
  {
    _jwtProvider = jwtProvider;
  }

  protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
  {
    Token = string.IsNullOrEmpty(Token) ? await _jwtProvider.GetAuthorizationHeaderAsync() : Token;
    return new HeaderParameter(KnownHeaders.Authorization, Token);
  }
}
