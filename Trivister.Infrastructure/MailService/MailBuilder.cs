using Trivister.ApplicationServices.Dto;

namespace Trivister.Infrastructure.MailService;

public sealed class MailBuilder: IMailBuilder
{
    private string _otpMessage;
    private string _otpSubject;
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
    
    public MailBuilder WithOTPMessage(string otpMessage, string otp, string email)
    {
        _otpMessage = string.Format(otpMessage, email, otp);
        return this;
    }

    public MailBuilder WithOTPSubject(string otpSubject)
    {
        _otpSubject = otpSubject;
        return this;
    }

    public MailObject BuildOtpMailDto()
    {
        return new MailObject()
        {
            BodyAmp = _otpMessage,
            CharSet = "utf-8",
            From = _fromEmail,
            IsTransactional = true,
            To = _toEmail,
            Sender = _fromEmail,
            Subject = _otpSubject
        };
        
        // return new MailRoot()
        // {
        //     Recipients = new Recipients()
        //     {
        //         To = new List<string>()
        //         {
        //             _toEmail
        //         }
        //     },
        //     Content = new Content()
        //     {
        //         Body = new List<MailBodyBody>()
        //         {
        //             new MailBodyBody()
        //             {
        //                 Content = _otpMessage,
        //                 Charset = "utf-8",
        //                 ContentType = "HTML"
        //             }
        //         },
        //         Subject = _otpSubject,
        //         From = _fromEmail
        //     }
        // };
    }

    public static implicit operator MailObject(MailBuilder builder)
    {
        return builder.BuildOtpMailDto();
    }
}