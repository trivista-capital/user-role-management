using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Abstractions;

public interface IGetTokenClient
{
    Task<TokenResult> GetToken(string username, string password);
}