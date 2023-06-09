namespace Trivister.ApplicationServices.Dto;

public class AddCustomerCommand
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    public string Address { get; set; }
    public string Sex { get; set; }
    public string Dob { get; set; }
    
    public Guid RoleId { get; set; }
}