using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Common.Model;
using IdentityOptions = Trivister.Common.Options.IdentityOptions;

namespace Trivister.Infrastructure.Http;

public class GetTokenClient: IGetTokenClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsMonitor<IdentityOptions> _identityOption;
    private readonly ILogger<GetTokenClient> _logger;
    public GetTokenClient(HttpClient httpClient, IOptionsMonitor<IdentityOptions> identityOption, ILogger<GetTokenClient> logger)
    {
        _httpClient = httpClient;
        _identityOption = identityOption;
        _logger = logger;
    }   
    
    public async Task<TokenResult> GetToken(string username, string password)
    {
        _logger.LogInformation("Going to authenticate the user with login command");
        var disco = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
        {
            Address = $"{_identityOption.CurrentValue.IdentityUrl}connect/token",
            ClientId = "MobileClient",
            ClientSecret = "secret",
            Scope = "fullAccess",
            Password = password,
            UserName = username
        });
        _logger.LogInformation("Got back from login command with response: {Response}", disco);
        if(disco != null)
            return new TokenResult()
            {
                AccessToken = disco.AccessToken,
                ExpiresIn = disco.ExpiresIn,
                Error = disco.Error,
                ErrorMessage = disco.ErrorDescription
            };
        return  new TokenResult()
        {
            Error = disco.Error,
            ErrorMessage = disco.ErrorDescription
        };
    }
}