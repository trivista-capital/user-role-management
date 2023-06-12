using System.Web;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.Common.Extensions;
using Trivister.Common.Model;
using Trivister.Core.Entities;

namespace Trivister.DataStore.Repositories;

public class IdentityService: IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IGlobalTSDbContext _context;

    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
        IPublisher publisher, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, 
        IAuthorizationService authorizationService, ILogger<IdentityService> logger, 
        IGlobalTSDbContext context)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _roleManager = roleManager;
        _logger = logger;
        _context = context;
    }
    
        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            return user!.UserName;
        }
        

        public async Task<ErrorResult<bool>> ValidateUser(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return ErrorResult.Fail<bool>("Please check user and try again");
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            return !isPasswordValid ? ErrorResult.Fail<bool>("Authentication failed for user") : ErrorResult.Ok(true);
        }
        
        public async Task<ErrorResult<ApplicationUser>> ValidateApplicationUser(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null) return ErrorResult.Fail<ApplicationUser>("username or password invalid");
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if(!isPasswordValid) return ErrorResult.Fail<ApplicationUser>("username or password invalid");
            return ErrorResult.Ok(user);

        }

        public async Task<ApplicationUser> GetUserNameByUserNameAsync(string userName, Guid? UserId = null)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName || u.PhoneNumber == userName || u.Id == UserId);
            return user!;
        }

        public async Task<ApplicationUser> GetUserById(Guid userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user!;
        }
        
        public async Task<ErrorResult<ApplicationUser>> GetUserByEmail(string email)
        {
            var user = await _userManager.Users.Where(u => u.Email == email).Select(x => x).FirstOrDefaultAsync();
            if (user == null) return ErrorResult.Fail<ApplicationUser>("Check user and try again");

            return ErrorResult.Ok(user);
        }
        

        public async Task<ErrorResult<ApplicationUser>> GetUserByEmailOrPhone(string username)
        {
            var user = await _userManager.Users.Where(u => u.UserName.ToLower() == username.ToLower() || u.PhoneNumber == username).Select(x => x).FirstOrDefaultAsync();
            if (user == null) return ErrorResult.Fail<ApplicationUser>("Check user and try again");

            return ErrorResult.Ok(user);
        }

        public IQueryable<ApplicationUser> GetApplicationUsersIQueryable()
        {
            return _userManager.Users;
        }

        public async Task<ErrorResult> ConfirmEmail(string email, string token)
        {
            _logger.LogInformation($"Confirm email url encoded token: {token} for email: {email}");
            var user = await GetUserByEmail(email);
            if (string.IsNullOrEmpty(user.Value.UserName)) return ErrorResult.Fail("Email might be wrong");
            _logger.LogInformation($"Confirm email url decoded token by browser: {token} for email: {email}");
            var emailVerificationResult = await _userManager.ConfirmEmailAsync(user.Value, token);
            _logger.LogInformation("Result of email verification is: {@RESULT}", emailVerificationResult.Succeeded);
            if (emailVerificationResult.Succeeded)
            {
                _logger.LogInformation($"Publishing EmailVerifiedMessage for email: {email}");
                // await _publisher.Publish(new LeatherbackSharedLibrary.Messages.EmailVerifiedMessage()
                // {
                //     CustomerId = user.Value.Id
                // });
                // _logger.LogInformation($"Published EmailVerifiedMessage for email: {email}");
                return ErrorResult.Ok();
            }
            
            return ErrorResult.Fail($"Unable to verify email {email}");
        }
        

        public async Task<(ErrorResult Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            // user.TwoFactorEnabled = true;
            var user = ApplicationUser.Factory.Create(userName, password);
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded ? (ErrorResult.Ok(), user.Id.ToString()) : (ErrorResult.Fail("Unable to create user"), user.Id.ToString());
        }

        public async Task<(ErrorResult Result, ApplicationUser user)> CreateUserAsync(Guid id, string firstName, string lastName, string email, string password)
        {
            var doesUserExist = await _userManager.FindByNameAsync(email);
            if (doesUserExist != null) return (ErrorResult.Fail("Duplicate user"), ApplicationUser.Factory.Create());
            var user = ApplicationUser.Factory.Create(id, firstName,  lastName,  email);
            var result = await _userManager.CreateAsync(user, password);
            if (result == null)
            {
                _logger.LogError("Unable to register user for some reason in IdentityService.CreateUserAsync method");
                throw new Exception();
            }
            if(result.Succeeded) return (ErrorResult.Ok(), user);
            var errorArray = result.Errors.Select(x => x.Description).ToArray();
            var error = string.Join(";", errorArray);
            return (ErrorResult.Fail(error), user);
        }
        

        public async Task<ErrorResult> CreateUserAsync(string password, ApplicationUser user)
        {
            // user.TwoFactorEnabled = true; //for now
            user.UserName = user.Email;
            user.SetCreatedOn();
            var checkExist = await _userManager.Users.AnyAsync(o => o.PhoneNumber == user.PhoneNumber || o.Email.ToLower() == user.Email.ToLower());
            if (checkExist)
                return (ErrorResult.Fail("Email or Phone number already exists"));
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded ? ErrorResult.Ok() : ErrorResult.Fail(result.Errors.FirstOrDefault()?.Description!);
        }
        
        
        public async Task<(ErrorResult Result, IdentityResult result, string UserId)> CreateUserAsync(ApplicationUser user, string password)
        {
            // user.TwoFactorEnabled = true;
            user.UserName = user.Email;
            user.SetCreatedOn();
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded ? (ErrorResult.Ok(), result, user.Id.ToString()) : (ErrorResult.Fail("Unable to create user"), result, user.Id.ToString());
        }
        
        
        public async Task<(ErrorResult Result, IdentityResult result, string UserId)> CreateUserAsync(ApplicationUser user)
        {
            // user.TwoFactorEnabled = true;
            user.UserName = user.Email;
            user.EmailConfirmed = false;
            user.SetCreatedOn();
            var result = await _userManager.CreateAsync(user);
            return result.Succeeded ? (ErrorResult.Ok(), result, user.Id.ToString()) : (ErrorResult.Fail("Unable to create user"), result, user.Id.ToString());
        }

        public async Task<ErrorResult> ChangePasswordAsync(ApplicationUser user, string newPassword)
        {
            // user.TwoFactorEnabled = true; //for now
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            return result.Succeeded ? ErrorResult.Ok("Password changed successfully") : ErrorResult.Fail(result.Errors.FirstOrDefault()?.Description!);
        }
        
        
        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            return await _userManager.IsInRoleAsync(user!, role);
        }
        
        
        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user!);
            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }
        
        public async Task<ErrorResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return ErrorResult.Ok();
        }

        private async Task<ErrorResult> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return ErrorResult.Ok();
        }

        public async Task<ErrorResult> IsUserEmailVerified(ApplicationUser user)
        {
            var result = await _userManager.IsEmailConfirmedAsync(user);
            if (!result) return ErrorResult.Fail("Email is not confirmed");
            return ErrorResult.Ok();
        }

        public async Task<ErrorResult<string>> GenerateEmailConfirmationLink(ApplicationUser user)
        {
            _logger.LogInformation("Generating email confirmation link for user, {@USER}", user.Email);
            var confirmationLink = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            _logger.LogInformation("Email confirmation link before encoding is: {@LINK} ", confirmationLink);
            var encodedLink = HttpUtility.UrlEncode(confirmationLink);
            _logger.LogInformation("Email confirmation link after encoding is: {@LINK} ", encodedLink);
            return ErrorResult.Ok<string>(encodedLink);
        }

        public async Task<ErrorResult<string>> GeneratePasswordResetTokenLink(ApplicationUser user)
        {
            _logger.LogInformation("Generating password reset link for user, {@USER}", user.Email);
            var resetLink = await _userManager.GeneratePasswordResetTokenAsync(user);

            _logger.LogInformation("Password reset before encoding is: {@LINK} ", resetLink);
            var encodedLink = HttpUtility.UrlEncode(resetLink);
            _logger.LogInformation("Password reset after encoding is: {@LINK} ", encodedLink);

            return ErrorResult.Ok<string>(encodedLink);
        }

        public async Task<ErrorResult<IdentityResult>> ResetPassword(ApplicationUser user, string token, string newPassword)
        {
            _logger.LogInformation("Reset password for user is: {@USER}", user.Email);
            _logger.LogInformation("Password reset link after decoding by browser is: {@LINK} ", token);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            _logger.LogInformation("Result of password reset is: {@RESULT} ", result.Succeeded);
            if (!result.Succeeded) return ErrorResult.Fail<IdentityResult>("Unable to reset password.");
            return ErrorResult.Ok(result);
        }

        public async Task<ErrorResult<IdentityResult>> AccountActivationAsync(ApplicationUser user, string currentPassword, string newPassword) 
        {
            _logger.LogInformation("Reset password for user is: {@USER}", user.Email);         
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded) return ErrorResult.Fail<IdentityResult>("Unable to reset password.");
            return ErrorResult.Ok(result);
        }
        

        public async Task<ErrorResult<bool>> RemoveUserFromRole(ApplicationUser applicationUser, string roleName)
        {
            _logger.LogInformation($"Removing User with email: {applicationUser.Email} from Role {roleName}");
            var roleExist =
                await _roleManager.Roles.FirstOrDefaultAsync(a => a.Name.Trim().ToLower() == roleName.Trim().ToLower());
            if (roleExist != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(applicationUser, roleExist.Name);
                if (result.Succeeded)
                {
                    return ErrorResult.Ok(true);
                }
            }

            return ErrorResult.Fail<bool>($"Role: {roleName} does not exist");
        }
        
        public async Task<ErrorResult<(Guid, bool)>> AddUserToRole(ApplicationUser applicationUser, string roleName)
        {
            var predicate = UserPredicate.GetUseRleByName(roleName);
            _logger.LogInformation($"Adding User with email: {applicationUser.Email} to Role {roleName}");
            var roleExist = await _roleManager.Roles.FirstOrDefaultAsync(predicate);
            if (roleExist == null) return ErrorResult.Fail<(Guid, bool)>($"Role: {roleName} does not exist");
            var usersRole = UsersRole.Factory.Create(applicationUser.Id, roleExist.Id);
            await _context.UsersRole.AddAsync(usersRole);
            var isCreated = await _context.SaveChangesAsync(new CancellationToken());
            if (isCreated > 0)
            {
                return ErrorResult.Ok((roleExist.Id, true));
            }
            return ErrorResult.Fail<(Guid, bool)>($"Role: {roleName} does not exist");
        }

        public async Task<ErrorResult<bool>> IsUserDisabled(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return ErrorResult.Fail<bool>("User can not be found");
            return user.IsDisabled ? ErrorResult.Fail<bool>("User is disabled") : ErrorResult.Ok(true);
        }
        
        public async Task<ErrorResult<bool>> IsUserLocked(string userName)
        {
            var user = await GetUserNameByUserNameAsync(userName);
            if(string.IsNullOrEmpty(user.UserName)) return ErrorResult.Fail<bool>("Please check user and try again");
            var isUserLocked = await _userManager.IsLockedOutAsync(user);
            return isUserLocked ? ErrorResult.Fail<bool>("User is locked out") : ErrorResult.Ok(true);
        }

        public async Task<ErrorResult<bool>> AddOrEditRole(Guid id, string name, string description)
        {
            var applicationRole = await GetRoleById(id);
            // check if role exist.
            if (applicationRole == null)
                throw new NotFoundException("Unable to get role"); 
            applicationRole.SetRoleNme(name).SetDescription(description).SetId(id).SetLastModified().SetNormalizedName(name);
            var result = await _roleManager.UpdateAsync(applicationRole);
            return result.Succeeded ? ErrorResult.Ok(true) : ErrorResult.Fail<bool>("Editing Role Failed");
        }

        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        
        public async Task<ApplicationRole> GetRoleById(Guid roleId)
        {
            var role = await _roleManager.Roles.Where(x=>x.Id == roleId).FirstOrDefaultAsync();
            return role;
        }
        
        public async Task<ApplicationRole> GetUsersRole(Guid userId)
        {
            var predicate = UserPredicate.GetUserById(userId);
            var userRole = await _context?.UsersRole?.Where(predicate).FirstOrDefaultAsync();
            if (userRole == null) return ApplicationRole.Factory.Create();
            var role = await _roleManager?.FindByIdAsync(userRole!.RoleId.ToString());
            return role;
        }
        
        public IQueryable<ApplicationRole> GetAllRoles()
        {
            var roles = _roleManager.Roles;
            //var roles = queryable.AsEnumerable().ToList();
            //var roles = await Task.FromResult(queryable.ToList());
            //return await roles.AsQueryable().ToListAsyncSafe<ApplicationRole>();
            //return new List<ApplicationRole>();
            return roles;
        }
        

        public async Task<ErrorResult<bool>> ResetPasswordAsync(ApplicationUser user, string code, string password)
        {
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded)
            {
                return ErrorResult.Ok(true);
            }

            var errormessage = string.Join(',', string.Join("; ", result.Errors.Select(x => x.Description)));

            return ErrorResult.Fail<bool>(errormessage.ToString());
        }

        public async Task<ErrorResult<bool>> CreateRoleAsync(ApplicationRole mappedRole)
        {
            if (!await _roleManager.RoleExistsAsync(mappedRole.Name))
            {
                mappedRole.NormalizedName = mappedRole.Name.ToUpper();
                var result = await _roleManager.CreateAsync(mappedRole);
                return result.Succeeded ? ErrorResult.Ok(true) : ErrorResult.Fail<bool>("Adding Role Failed");
            }
            return ErrorResult.Ok(true);
        }
        
        public async Task<ErrorResult<(Guid, bool)>> CreateRoleReturnIdAsync(ApplicationRole mappedRole)
        {
            Guid id = Guid.Empty;
            if (!await _roleManager.RoleExistsAsync(mappedRole.Name))
            {
                mappedRole.NormalizedName = mappedRole.Name.ToUpper();
                var result = await _roleManager.CreateAsync(mappedRole);
                id = mappedRole.Id;
                return result.Succeeded ? ErrorResult.Ok((mappedRole.Id, true)) : ErrorResult.Fail<(Guid, bool)>("Adding Role Failed");
            }
            return ErrorResult.Ok((id, true));
        }

        public async Task<ErrorResult<bool>> UpdateUserAsync(ApplicationUser applicationUser)
        {
            applicationUser.SetLastModifiedDate();
            var result = await _userManager.UpdateAsync(applicationUser);
            return result.Succeeded ? ErrorResult.Ok(true) : ErrorResult.Fail<bool>("User is locked out");
        }
        
        public async Task<ErrorResult<bool>> AssignPermissionsToRole(Guid roleId, List<int> permissionIds)
        {
            var rolePermissionsToDelete = new List<RolesPermission>();
            
            var rolesPermissions =  await _context?.RolesPermissions?
                                                    .Include(x=>x.Permission)
                                                    .Include(x=>x.Role)
                                                    .Where(x=>x.RoleId == roleId).Select(x=>x).ToListAsync();

            foreach (var permission in rolesPermissions)
            {
                var rolePermissionToDelete = await _context?.RolesPermissions?.Where(x => x.PermissionId == permission.PermissionId).Select(x => x).FirstOrDefaultAsync();
                rolePermissionsToDelete.Add(rolePermissionToDelete);
            }

            _context.RolesPermissions.RemoveRange(rolePermissionsToDelete);
            await _context.SaveChangesAsync(new CancellationToken());

            var rolePermissionList = permissionIds.Select(permissionId => new RolesPermission { RoleId = roleId, PermissionId = permissionId }).ToList();

            await _context.RolesPermissions.AddRangeAsync(rolePermissionList);
            var saved = await _context.SaveChangesAsync(new CancellationToken());
            return saved > 0 ? ErrorResult.Ok(true) : ErrorResult.Fail<bool>("Unable to permissions to role");
        }
}