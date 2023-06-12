using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Dto;

namespace Trivister.Infrastructure.Http;

public class CustomerClient: ICustomerClient
{
    private readonly ILogger<CustomerClient> _logger;
    private readonly HttpClient _client;
    
    public CustomerClient(HttpClient client, ILogger<CustomerClient> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task PublishCustomer(AddCustomerCommand customer)
    {
        try
        {
            var body = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var httpResult = await _client.PostAsync("addCustomer", body);
            var result = await httpResult.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject(result);
            if (httpResult.IsSuccessStatusCode)
            {
                _logger.LogInformation("Creation of Customer was pushed successfully wth response : {Response}", deserializedResponse);
            }
            _logger.LogInformation("Publishing of customer was not successful with message: {Message}", deserializedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task PublishRole(CreateRoleCommand role)
    {
        _logger.LogInformation("Entered the PublishRole http method");
        try
        {
            var body = new StringContent(JsonConvert.SerializeObject(role), Encoding.UTF8, "application/json");
            _logger.LogInformation("About posting to loan app");
            var httpResult = await _client.PostAsync("addRole", body);
            _logger.LogInformation("Posted to loan app");
            var result = await httpResult.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject(result);
            if (httpResult.IsSuccessStatusCode)
            {
                _logger.LogInformation("Creation of role was pushed successfully wth response : {Response}", deserializedResponse);
            }
            _logger.LogInformation("Publishing of role was not successful with message: {Message}", deserializedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}