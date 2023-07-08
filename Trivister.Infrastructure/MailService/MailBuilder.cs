using Trivister.ApplicationServices.Common.Helper;
using Trivister.ApplicationServices.Dto;

namespace Trivister.Infrastructure.MailService;

public sealed class MailBuilder: IMailBuilder
{
    private string _message;
    private string _subject;
    private string _toEmail;
    private string _fromEmail;

    public MailBuilder WithToEmail(string toEmail)
    {
        _toEmail = toEmail;
        return this;
    }
    
    public MailBuilder WithFromEmail(string fromEmail)
    {
        _fromEmail = fromEmail;
        return this;
    }
    
    public MailBuilder WithSignUpMessageMessage(string name, string otp)
    {
        var rawMailTemplate = EmailTemplateHelper.ExtractMailTemplate("page1-sign-up-otp.html");
        _message = string.Format(rawMailTemplate, name, otp);
        return this;
    }

    public MailBuilder WithForgotPasswordMessage(string name, string confirmationLink)
    {
        var rawMailTemplate = EmailTemplateHelper.ExtractMailTemplate("page2-forgot-password-otp.html");
        _message = string.Format(rawMailTemplate, name, confirmationLink);
        return this;
    }

    public MailBuilder WithPasswordSuccessfullyResetMessage(string name)
    {
        var rawMailTemplate = EmailTemplateHelper.ExtractMailTemplate("page3-password-reset-successfully.html");
        _message = string.Format(rawMailTemplate, name);
        return this;
    }
    
    public MailBuilder WithWelcomeMessage(string name)
    {
        var rawMailTemplate = EmailTemplateHelper.ExtractMailTemplate("page4-welcome-to-borrowease.html");
        _message = rawMailTemplate.Replace("{Customer's Name}", name);
        return this;
    }
    
    public MailBuilder WithAdminUserInvitationMessage(string adminName)
    {
        var rawMailTemplate = EmailTemplateHelper.ExtractMailTemplate("page13-new-admin-user.html");
        _message = rawMailTemplate.Replace("{Admin's Name}", adminName);
        return this;
    }
    
    public MailBuilder WithMessageToAdminOnCustomerRegistrationMessage(string adminName, string customerFullName, string customerEmail, string customerPhone, string dateOfRegistration)
    {
        var rawMailTemplate = EmailTemplateHelper.ExtractMailTemplate("page15-new-customer-message-to-admin.html");
        _message = rawMailTemplate.Replace("{Admin's Name}", adminName)
                                  .Replace("{Customer's Full Name}", customerFullName)
                                  .Replace("{Customer's Email Address}", customerEmail)
                                  .Replace("{Customer's Phone Number}", customerPhone)
                                  .Replace("{Date of Registration}", dateOfRegistration);
        return this;
    }

    public MailBuilder WithOTPSubject(string otpSubject)
    {
        _subject = otpSubject;
        return this;
    }

    public MailObject BuildOtpMailDto()
    {
        return new MailObject()
        {
            BodyAmp = _message,
            CharSet = "utf-8",
            From = _fromEmail,
            IsTransactional = true,
            To = _toEmail,
            Sender = _fromEmail,
            Subject = _subject
        };
    }

    public static implicit operator MailObject(MailBuilder builder)
    {
        return builder.BuildOtpMailDto();
    }
}