using Okta.Sdk.Model;

namespace IdentityBridge.Services;

public interface IUserService
{
    Task<object> GetAsync(string id);
    Task<List<User>> GetAsync();
}
