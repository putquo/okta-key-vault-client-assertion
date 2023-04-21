using IdentityBridge.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Okta.Sdk.Client;
using Okta.Sdk.Model;

namespace IdentityBridge.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _okta;

    public UsersController(ILogger<UsersController> logger, IUserService okta)
    {
        _logger = logger;
        _okta = okta;
    }

    [HttpGet("{id}")]
    public async Task<object> Get(string id)
    {
        try
        {
          _logger.LogDebug("action={}, user={}", Request.GetDisplayUrl(), id);
          var user = await _okta.GetAsync(id);
          return user;
        }
        catch (ApiException ex)
        {
          _logger.LogError(ex.Message);
          return new User();
        }
    }
}
