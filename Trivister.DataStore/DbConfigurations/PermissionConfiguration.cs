using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trivister.Core.Entities;

namespace Trivister.DataStore.DbConfigurations;

public class PermissionConfiguration: IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasData(new Permission() { 
                Name = "CanAddUser", 
                Description = "Can add user to the system", 
                Id = 1
            },new Permission(){ 
                Name = "CanDeleteUser", 
                Description = "Can delete user",
                Id = 2
            },new Permission(){ 
                Name = "CanEditUser", 
                Description = "Can edit user",
                Id = 3
            },new Permission(){ 
                Name = "CanInviteUser", 
                Description = "Can invite user",
                Id = 4
            }, new Permission(){ 
                Name = "CanViewUsers", 
                Description = "Can View user",
                Id = 5
            },new Permission(){ 
                Name = "CanViewLoans", 
                Description = "Can View Loans",
                Id = 6
            },new Permission(){ 
                Name = "CanApproveLoans", 
                Description = "Can Approve Loans",
                Id = 7
            },new Permission(){ 
                Name = "CanRejectLoans", 
                Description = "Can Reject Loans",
                Id = 8
            },new Permission(){ 
                Name = "CanWriteOffLoans", 
                Description = "Can WriteOff Loans",
                Id = 9
            },new Permission(){ 
                Name = "CanViewReports", 
                Description = "Can View Reports",
                Id = 10
            },new Permission(){ 
                Name = "CanDownloadReports", 
                Description = "Can Download Reports",
                Id = 11
            },new Permission(){ 
                Name = "CanCreateRole", 
                Description = "Can Create Role",
                Id = 12
            },new Permission(){ 
                Name = "CanUpdateRole", 
                Description = "Can Update Role",
                Id = 13
            },new Permission(){ 
                Name = "CanAddPermissionsToRole", 
                Description = "Can Add permissions to Role",
                Id = 14
            },new Permission(){ 
                Name = "CanViewRoles", 
                Description = "Can View Roles",
                Id = 15
            },new Permission(){ 
                Name = "CanViewTickets", 
                Description = "Can View Tickets",
                Id = 16 
            },new Permission(){ 
                Name = "CanViewTickets", 
                Description = "Can View Tickets",
                Id = 17
            },new Permission(){ 
                Name = "CanOpenOrCloseTicket", 
                Description = "Can Open/Close Tickets",
                Id = 18 
            },new Permission(){ 
                Name = "CanRespondToTicket", 
                Description = "Can Respond to Tickets",
                Id = 19 
            },new Permission(){ 
                Name = "CanViewConfigurations", 
                Description = "Can View Configurations",
                Id = 20 
            },new Permission(){ 
                Name = "CanUpdateMakerCheckerConfiguration", 
                Description = "Can Update Maker Checker Configurations",
                Id = 21 
            },new Permission(){ 
                Name = "CanCreateLoanConfiguration", 
                Description = "Can Create Loan Configuration",
                Id = 22 
            },new Permission(){ 
                Name = "CanUpdateLoanConfiguration", 
                Description = "Can Update Loan Configuration",
                Id = 23 
            },new Permission(){ 
                Name = "CanDeleteLoanConfiguration", 
                Description = "Can Delete Loan Configuration",
                Id = 24 
        });
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.Description).HasColumnType("nvarchar(400)");
    }
}