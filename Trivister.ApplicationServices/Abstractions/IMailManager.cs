namespace Trivister.ApplicationServices.Abstractions;

public interface IMailManager
{
    void BuildOTPMessage(string otp, string email);
}