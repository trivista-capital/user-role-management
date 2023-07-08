using System.Globalization;

namespace Trivister.ApplicationServices.Common.Options;

public class MailOptions
{
    public string APIKey { get; set; } 
    public string From { get; set; }
    
    public string OTPMailTemplate { get; set; }
    public string PasswordResetMailTemplate { get; set; }
    public string OTPMailSubject { get; set; }
    public string PasswordResetSubject { get; set; }
    public string WelcomeMessageSubject { get; set; }
    public string AdminWelcomeSubject { get; set; }
    public string AdminNotificatinOfCustomerRegistrationSubject { get; set; }
    public string BaseAddress { get; set; } 
    
    public string MailSubPath { get; set; } 
}