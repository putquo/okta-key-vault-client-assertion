namespace IdentityBridge.Builders;

public interface IJwtBuilder
{
  IJwtBuilder WithAudience(string aud);
  IJwtBuilder WithIssuer(string iss);
  IJwtBuilder WithKeyId(string kid);
  IJwtBuilder WithSubject(string sub);
  Task<string> Build();
}
