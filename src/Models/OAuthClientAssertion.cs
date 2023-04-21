using RestSharp;
using static IdentityBridge.Constants.OAuthConstants;

namespace IdentityBridge.Models;


public class OAuthClientAssertionRequest
{
  public string grant_type { get; set; } = GrantTypes.ClientCredentials;
  public string client_assertion_type { get; set; } = ClientAssertionTypes.JwtBearer;
  public string client_assertion { get; set; } = string.Empty;
  public string scope { get; set; } = string.Empty;
}
