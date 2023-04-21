using System.ComponentModel.DataAnnotations;

namespace IdentityBridge;

public class ClientAssertionOptions
{
    [Required, Url]
    public Uri Audience { get; set; } = default!;
    [Required]
    public string Issuer { get; set; } = default!;
    [Required]
    public string KeyId { get; set; } = default!;
    [Required]
    public List<string> Scopes { get; set; } = default!;
    [Required]
    public string Subject { get; set; } = default!;
}

public class OktaOptions
{
    [Required, Url]
    public Uri Uri { get; set; } = default!;
    [Required]
    public string TokenEndpoint { get; set; } = default!;
    [Required]
    public string UsersEndpoint { get; set; } = default!;
    [Required]
    public ClientAssertionOptions ClientAssertion { get; set; } = default!;
}
