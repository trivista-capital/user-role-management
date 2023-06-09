namespace Trivister.Common.Model;

public class TokenResult
{
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string ErrorMessage { get; set; }
    public string Error { get; set; }
}