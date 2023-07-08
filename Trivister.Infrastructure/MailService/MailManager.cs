using System.Diagnostics;
using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Common.Helper;
using Trivister.ApplicationServices.Common.Options;
using Trivister.ApplicationServices.Dto;

namespace Trivister.Infrastructure.MailService;

public sealed class MailManager: IMailManager
{
    private static MailOptions _mailOptions;
    private readonly HttpClient _client;
    private readonly ILogger<MailManager> _logger;

    public MailManager(IOptions<MailOptions> mailOptions, HttpClient client, ILogger<MailManager> logger)
    {
        _client = client;
        _logger = logger;
        _mailOptions = mailOptions.Value;
    }
    
    public void BuildSignUpMessage(string otp, string to, string name)
    {
        var builder = new MailBuilder();
        var mailObject = builder.WithToEmail(to)
            .WithFromEmail(_mailOptions.From)
            .WithOTPSubject(_mailOptions.OTPMailSubject)
            .WithSignUpMessageMessage(name, otp)
            .BuildOtpMailDto(); 
        SendEmail(mailObject);
    }
    
    public void BuildPasswordSuccessfullyResetMessage(string to, string name)
    {
        var builder = new MailBuilder();
        var mailObject = builder.WithToEmail(to)
            .WithFromEmail(_mailOptions.From)
            .WithOTPSubject(_mailOptions.OTPMailSubject)
            .WithPasswordSuccessfullyResetMessage(name)
            .BuildOtpMailDto(); 
        SendEmail(mailObject);
    }

    public void BuildForgotPasswordMessage(string message, string to)
    {
        var builder = new MailBuilder();
        var mailObject = builder.WithToEmail(to)
            .WithFromEmail(_mailOptions.From)
            .WithOTPSubject(_mailOptions.PasswordResetSubject)
            .WithForgotPasswordMessage(_mailOptions.PasswordResetMailTemplate, message)
            .BuildOtpMailDto(); 
        SendEmail(mailObject);
    }

    public void BuildResetPasswordMessage(string confirmationLink, string to)
    {
        var builder = new MailBuilder();
        var mailObject = builder.WithToEmail(to)
            .WithFromEmail(_mailOptions.From)
            .WithOTPSubject(_mailOptions.PasswordResetSubject)
            .WithForgotPasswordMessage(confirmationLink, to)
            .BuildOtpMailDto(); 
        SendEmail(mailObject);
    }
    
    public void BuildWelcomeMessage(string name, string to)
    {
        var builder = new MailBuilder();
        var mailObject = builder.WithToEmail(to)
            .WithFromEmail(_mailOptions.From)
            .WithOTPSubject(_mailOptions.WelcomeMessageSubject)
            .WithWelcomeMessage(name)
            .BuildOtpMailDto(); 
        SendEmail(mailObject);
    }
    
    public void BuildAdminUserInvitationMessage(string to, string adminName)
    {
        var builder = new MailBuilder();
        var mailObject = builder.WithToEmail(to)
            .WithFromEmail(_mailOptions.From)
            .WithOTPSubject(_mailOptions.AdminWelcomeSubject)
            .WithAdminUserInvitationMessage(adminName)
            .BuildOtpMailDto(); 
        SendEmail(mailObject);
    }

    public void BuildMessageToAdminOnCustomerRegistrationMessage(string to, string adminName, string customerFullName,
        string customerEmail, string customerPhone, string dateOfRegistration)
    {
        var builder = new MailBuilder();
        var mailObject = builder.WithToEmail(to)
            .WithFromEmail(_mailOptions.From)
            .WithOTPSubject(_mailOptions.AdminNotificatinOfCustomerRegistrationSubject)
            .WithMessageToAdminOnCustomerRegistrationMessage(adminName,  customerFullName, customerEmail,  customerPhone, dateOfRegistration)
            .BuildOtpMailDto(); 
        SendEmail(mailObject);
    }

    private void SendEmail(MailObject dto)
    { 
        Configuration config = new Configuration();
        // Configure API key authorization: apikey
        config.ApiKey.Add("X-ElasticEmail-ApiKey", _mailOptions.APIKey);
        var apiInstance = new EmailsApi(config);
        var to = new List<string> { dto.To };
        var recipients = new TransactionalRecipient(to: to);
        EmailTransactionalMessageData emailData = new EmailTransactionalMessageData(recipients: recipients)
        {
             Content = new EmailContent
             {
                 Body = new List<BodyPart>()
             }
        };
        BodyPart htmlBodyPart = new BodyPart
        {
            ContentType = BodyContentType.HTML,
            Charset = "utf-8",
            Content = dto.BodyAmp
        };
        BodyPart plainTextBodyPart = new BodyPart
        {
            ContentType = BodyContentType.PlainText,
            Charset = "utf-8",
            Content = dto.BodyAmp
        };
        emailData.Content.Body.Add(htmlBodyPart);
        emailData.Content.Body.Add(plainTextBodyPart);
        emailData.Content.From = dto.From;
        emailData.Content.Subject = dto.Subject;

        try
        {
            // Send Bulk Emails
            var result = apiInstance.EmailsTransactionalPost(emailData);
            Debug.WriteLine(result);
        }
        catch (ApiException  e)
        {
            Debug.Print("Exception when calling EmailsApi.EmailsPost: " + e.Message);
            Debug.Print("Status Code: " + e.ErrorCode);
            Debug.Print(e.StackTrace);
        }
    }
}