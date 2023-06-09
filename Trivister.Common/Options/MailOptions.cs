namespace Trivister.Common.Options;

public class MailOptions
{
    public string From { get; set; } = string.Empty;
    public string Smtp { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
}