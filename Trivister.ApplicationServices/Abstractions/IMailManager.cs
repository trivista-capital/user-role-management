namespace Trivister.ApplicationServices.Abstractions;

public interface IMailManager
{
    void BuildSignUpMessage(string otp, string to, string name);
    void BuildForgotPasswordMessage(string message, string to);
    void BuildResetPasswordMessage(string confirmationLink, string to);
    void BuildPasswordSuccessfullyResetMessage(string to, string name);
    void BuildWelcomeMessage(string name, string to);
    void BuildAdminUserInvitationMessage(string to, string adminName);
    void BuildMessageToAdminOnCustomerRegistrationMessage(string to, string adminName, string customerFullName,
        string customerEmail, string customerPhone, string dateOfRegistration);
}