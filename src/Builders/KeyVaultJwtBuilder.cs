using IdentityBridge.Handlers;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace IdentityBridge.Builders;

public class KeyVaultJwtBuilder : IJwtBuilder
{
  private IDictionary<string, object> _payload;
  private IDictionary<string, object> _header;
  private readonly IJwtSignatureHandler _jwtSignatureHandler;

  public KeyVaultJwtBuilder(IJwtSignatureHandler jwtSignatureHandler)
  {
    _header = new Dictionary<string, object>()
    {
      { JwtHeaderParameterNames.Alg, SecurityAlgorithms.RsaSha256 },
      { JwtHeaderParameterNames.Typ, JwtConstants.HeaderType },
    };
    _payload = new Dictionary<string, object>()
    {
      { JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds() },
      { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() },
      { JwtRegisteredClaimNames.Nbf, DateTimeOffset.Now.ToUnixTimeSeconds() },
      { JwtRegisteredClaimNames.Exp, DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds() },
    };
    _jwtSignatureHandler = jwtSignatureHandler;
  }
  public IJwtBuilder WithAudience(string value)
  {
    _payload.Add(JwtRegisteredClaimNames.Aud, value);
    return this;
  }

  public IJwtBuilder WithIssuer(string value)
  {
    _payload.Add(JwtRegisteredClaimNames.Iss, value);
    return this;
  }

  public IJwtBuilder WithSubject(string value)
  {
    _payload.Add(JwtRegisteredClaimNames.Sub, value);
    return this;
  }

  public IJwtBuilder WithKeyId(string value)
  {
    _header.Add(JwtHeaderParameterNames.Kid, value);
    return this;
  }

  public async Task<string> Build()
  {
    var unsignedJwt = $"{Encode(_header)}.{Encode(_payload)}";
    return await _jwtSignatureHandler.SignAsync(unsignedJwt);
  }

  private static string Encode(IDictionary<string, object> keyValuePairs)
  {
    return Base64UrlEncoder.Encode(JsonSerializer.Serialize(keyValuePairs));
  }
}
