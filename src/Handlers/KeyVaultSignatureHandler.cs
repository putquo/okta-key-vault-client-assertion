using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using IdentityBridge.Handlers;
using IdentityBridge.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace IdentityBridge.Generators;

public class KeyVaultJwtSignatureHandler : IJwtSignatureHandler
{
  private readonly KeyClient _keyClient;
  private readonly IOptions<KeyVaultOptions> _options;

  public KeyVaultJwtSignatureHandler(IOptions<KeyVaultOptions> options, KeyClient keyClient)
  {
    _options = options;
    _keyClient = keyClient;
  }

  public async Task<string> SignAsync(string unsignedJwt)
  {
    var digest = GenerateDigest(unsignedJwt);
    var response = await _keyClient.GetCryptographyClient(_options.Value.SigningKeyName).SignAsync(SignatureAlgorithm.RS256, digest);
    return $"{unsignedJwt}.{Base64UrlEncoder.Encode(response.Signature)}";
  }

  private static byte[] GenerateDigest(string unsignedJwt)
  {
    var byteData = Encoding.UTF8.GetBytes(unsignedJwt);
    var hasher = SHA256.Create();
    return hasher.ComputeHash(byteData);
  }
}
