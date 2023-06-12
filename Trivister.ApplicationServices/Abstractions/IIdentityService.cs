using Microsoft.AspNetCore.Identity;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Abstractions;

public interface IIdentityService
{
        Task<ErrorResult<IdentityResult>> AccountActivationAsync(ApplicationUser user, string currentPassword, string newPassword); 
        Task<(ErrorResult Result, IdentityResult result, string UserId)> CreateUserAsync(ApplicationUser user);
        Task<(ErrorResult Result, IdentityResult result, string UserId)> CreateUserAsync(ApplicationUser user, string password);
        
        Task<(ErrorResult Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<(ErrorResult Result, ApplicationUser user)> CreateUserAsync(Guid id, string firstName,
                string lastName, string email, string password);
        Task<string> GetUserNameAsync(string userId);
        Task<ApplicationUser> GetUserNameByUserNameAsync(string userName, Guid? userId = null);
        Task<ErrorResult<ApplicationUser>> GetUserByEmail(string email);
        Task<ErrorResult> ConfirmEmail(string email, string token);
        Task<ErrorResult> IsUserEmailVerified(ApplicationUser user);
        Task<ApplicationUser> GetUserById(Guid userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<ErrorResult> DeleteUserAsync(string userId);
        Task<ErrorResult<string>> GenerateEmailConfirmationLink(ApplicationUser user);
        Task<ErrorResult<string>> GeneratePasswordResetTokenLink(ApplicationUser user);
        Task<ErrorResult<bool>> IsUserDisabled(string userName);
        Task<ErrorResult<bool>> IsUserLocked(string userName);
        Task<ErrorResult<bool>> ValidateUser(string username, string password);
        Task<ErrorResult<IdentityResult>> ResetPassword(ApplicationUser user, string token, string newPassword);
        Task<ApplicationRole> GetUsersRole(Guid userId);
        IQueryable<ApplicationRole> GetAllRoles();
        Task<ErrorResult<(Guid, bool)>> AddUserToRole(ApplicationUser mappedUser, string roleName);
        Task<ErrorResult<bool>> AddOrEditRole(Guid id, string name, string description);
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        Task<ApplicationRole> GetRoleById(Guid roleId);
        Task<ErrorResult<bool>> RemoveUserFromRole(ApplicationUser applicationUser, string roleName);
        Task<ErrorResult<bool>> ResetPasswordAsync(ApplicationUser user, string code, string password);
        Task<ErrorResult<bool>> CreateRoleAsync(ApplicationRole mappedRole);
        Task<ErrorResult<(Guid, bool)>> CreateRoleReturnIdAsync(ApplicationRole mappedRole);
        Task<ErrorResult<bool>> UpdateUserAsync(ApplicationUser applicationUser);
        Task<ErrorResult> CreateUserAsync(string password, ApplicationUser user);
        Task<ErrorResult<ApplicationUser>> GetUserByEmailOrPhone(string username);
        Task<ErrorResult> ChangePasswordAsync(ApplicationUser user, string newPassword);
        Task<ErrorResult<ApplicationUser>> ValidateApplicationUser(string username, string password);
        IQueryable<ApplicationUser> GetApplicationUsersIQueryable();
        Task<ErrorResult<bool>> AssignPermissionsToRole(Guid roleId, List<int> permissionIds);
}