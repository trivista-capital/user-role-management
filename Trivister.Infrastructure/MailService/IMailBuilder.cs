using Trivister.ApplicationServices.Dto;

namespace Trivister.Infrastructure.MailService;


public interface IMailBuilder
{
    MailBuilder WithToEmail(string toEmail);
    MailBuilder WithFromEmail(string fromEmail);
    MailBuilder WithSignUpMessageMessage(string message, string otp);
    MailBuilder WithForgotPasswordMessage(string confirmationLink, string otp);
    MailBuilder WithPasswordSuccessfullyResetMessage(string name);
    MailBuilder WithWelcomeMessage(string name);

    MailBuilder WithAdminUserInvitationMessage(string adminName);

    MailBuilder WithMessageToAdminOnCustomerRegistrationMessage(string adminName, string customerFullName,
        string customerEmail, string customerPhone, string dateOfRegistration);
    MailBuilder WithOTPSubject(string subject);
    MailObject BuildOtpMailDto();
}