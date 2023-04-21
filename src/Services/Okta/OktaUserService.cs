using Microsoft.Extensions.Options;
using Okta.Sdk.Model;
using RestSharp;

namespace IdentityBridge.Services.Okta;

public class OktaUserService : IUserService
{
    private readonly IOptions<OktaOptions> _options;
    private readonly RestClient _restClient;

    public OktaUserService(RestClient restClient, IOptions<OktaOptions> options)
    {
        _restClient = restClient;
        _options = options;
    }

    public async Task<object> GetAsync(string id)
    {
        try
        {
            var response = await _restClient.GetJsonAsync<object>($"{_options.Value.UsersEndpoint}/{id}");
            return response!;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new User();
        }
    }

    public Task<List<User>> GetAsync()
    {
        throw new NotImplementedException();
    }
}
