using Trivister.ApplicationServices.Dto;

namespace Trivister.Infrastructure.MailService;


public interface IMailBuilder
{
    MailBuilder WithToEmail(string toEmail);
    MailBuilder WithFromEmail(string fromEmail);
    MailBuilder WithOTPMessage(string message, string otp, string email);
    MailBuilder WithOTPSubject(string subject);
    MailObject BuildOtpMailDto();
}