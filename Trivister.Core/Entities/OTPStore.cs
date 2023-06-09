using System.Runtime.InteropServices.ComTypes;
using System.Security.AccessControl;

namespace Trivister.Core.Entities;

public sealed class OTPStore: BaseEntityOfT<Guid>
{
    internal OTPStore() { }
    private OTPStore(Guid id, string otp, string email, string phoneNumber, int minutes)
    {
        Id = id;
        OTP = otp;
        Email = email;
        PhoneNumber = phoneNumber;
        CreatedOn = DateTime.UtcNow;
        ExpiryDate = DateTime.UtcNow.AddMinutes(minutes);
        IsExpired = false;
        IsUsed = false;
        LastModified = DateTime.UtcNow;
    }
    
    public string OTP { get; private set; }
    
    public string Email { get; private set; }
    
    public string PhoneNumber { get; private set; }
    
    public DateTime ExpiryDate { get; set; }
    
    public bool IsExpired { get; set; }
    
    public bool IsUsed { get; set; }

    public class Factory
    {
        public static OTPStore GenerateOto(Guid id, string otp, string email, string phoneNumber, int minutes)
        {
            return new OTPStore(id, otp, email, phoneNumber, minutes);
        }
    }
    protected override void When(object @event)
    {
        throw new NotImplementedException();
    }
}