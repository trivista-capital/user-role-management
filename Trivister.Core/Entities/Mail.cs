namespace Trivister.Core.Entities;

public class Mail
{
    private Mail(string from, string to, string subject, string body)
    {
        From = from;
        To = to;
        Subject = subject;
        Body = body;
    }
    public string From { get; private set; }
    public string To { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    
    public static class Factory
    {
        public static Mail Create(string from, string to, string subject, string body)
        {
            return new Mail(from, to, subject, body);
        }
    }
}

public class OTPLogger
{
    public string OTP { get; set; }
}