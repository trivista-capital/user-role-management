using Trivister.ApplicationServices.Dto;

namespace Trivister.ApplicationServices.Abstractions;

public interface ICustomerClient
{
    Task PublishCustomer(AddCustomerCommand customer);
    Task PublishRole(CreateRoleCommand role);
}