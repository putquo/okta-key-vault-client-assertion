using IdentityBridge.Builders;
using IdentityBridge.Models;
using Microsoft.Extensions.Options;
using RestSharp;

namespace IdentityBridge.Services.Okta;

public class OktaOAuthService : IOAuthService
{
    private OktaOptions _configuration;
    private readonly IJwtBuilder _jwtBuilder;
    private readonly RestClient _restClient;

    public OktaOAuthService(
      RestClient restClient, IJwtBuilder jwtBuilder, IOptions<OktaOptions> options)
    {
        _restClient = restClient;
        _jwtBuilder = jwtBuilder;
        _configuration = options.Value;
    }

    public async Task<OAuthTokenResponse> GetBearerTokenAsync()
    {
        var clientAssertion = await _jwtBuilder.WithAudience(_configuration.ClientAssertion.Audience.ToString())
            .WithIssuer(_configuration.ClientAssertion.Issuer)
            .WithSubject(_configuration.ClientAssertion.Subject)
            .WithKeyId(_configuration.ClientAssertion.KeyId)
            .Build();
        var request = new RestRequest(_configuration.TokenEndpoint, Method.Post)
          .AddObject(new OAuthClientAssertionRequest
          {
              client_assertion = clientAssertion,
              scope = string.Join(" ", _configuration.ClientAssertion.Scopes),
          });
        try
        {
            var response = await _restClient.PostAsync<OAuthTokenResponse>(request);
            return response!;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new OAuthTokenResponse();
        }
    }
}
