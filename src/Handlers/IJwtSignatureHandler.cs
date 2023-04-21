namespace IdentityBridge.Handlers;

public interface IJwtSignatureHandler
{
  Task<string> SignAsync(string unsignedJwt);
}
