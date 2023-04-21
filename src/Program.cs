using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using IdentityBridge;
using IdentityBridge.Authenticators;
using IdentityBridge.Builders;
using IdentityBridge.Generators;
using IdentityBridge.Handlers;
using IdentityBridge.Options;
using IdentityBridge.Providers;
using IdentityBridge.Services;
using IdentityBridge.Services.Okta;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<KeyVaultOptions>()
    .BindConfiguration("KeyVault")
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<OktaOptions>()
    .BindConfiguration("Okta")
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton(sp => 
  new KeyClient(builder.Configuration.GetValue<Uri>("KeyVault:Uri"), new AzureCliCredential())
);
builder.Services.AddSingleton<IJwtSignatureHandler, KeyVaultJwtSignatureHandler>();
builder.Services.AddSingleton<IJwtBuilder, KeyVaultJwtBuilder>();
builder.Services.AddSingleton<IOAuthService, OktaOAuthService>(sp =>
{
  var client = new RestClient(builder.Configuration.GetValue<Uri>("Okta:Uri")!);
  var jwt = sp.GetRequiredService<IJwtBuilder>();
  var options = sp.GetRequiredService<IOptions<OktaOptions>>();
  return new OktaOAuthService(client, jwt, options);
});
builder.Services.AddSingleton<IJwtProvider, DefaultJwtProvider>();
builder.Services.AddSingleton<IUserService, OktaUserService>(sp =>
{
  var provider = sp.GetRequiredService<IJwtProvider>();
  var clientOptions = new RestClientOptions(builder.Configuration.GetValue<Uri>("Okta:Uri")!)
  {
    Authenticator = new OktaAuthenticator(provider)
  };
  var client = new RestClient(clientOptions, configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
  }));
  var options = sp.GetRequiredService<IOptions<OktaOptions>>();
  return new OktaUserService(client, options);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

