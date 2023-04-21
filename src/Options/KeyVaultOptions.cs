using System.ComponentModel.DataAnnotations;

namespace IdentityBridge.Options;

public class KeyVaultOptions
{
  [Required, Url]
  public Uri Uri { get; set; } = default!;
  [Required]
  public string SigningKeyName { get; set; } = default!;
}
