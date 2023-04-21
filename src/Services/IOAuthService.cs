using IdentityBridge.Models;

namespace IdentityBridge.Services;

public interface IOAuthService
{
    Task<OAuthTokenResponse> GetBearerTokenAsync();
}
