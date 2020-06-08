using Identity.Domain;
using Identity.Response;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Entities.Models;

namespace Identity.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> TenantRegisterAsync(string email, string password);
        Task<ResetPasswordResult> ForgotPassword(string email);
        Task<MainResponse> EmailResetPassword(string Email, string Password, string Token);
        Task<MainResponse> PasswordReset(string password, string token);
        Task<ChangeEmailResult> ChangeEmail(string newEmail, string token);
        Task<ChangeEmailSuccessResponse> ChangeEmailAddress(string newEmail, string password, string resetToken, string jwtToken);
        Task<bool> CheckIfEmailInUse(string Email);
        Task<bool> CheckIfTenantEmailInUse(string email);
        Task<DeleteUserResponse> DeleteUserById(int id, string jwtToken);
        Task<bool> CheckIfAspNetUserExistByAspNetUserId(int id);
        Task<AnonimizationResponse> AnonimizeUserByEmailFromToken(string token);
        Task<AnonimizationResponse> AnonimizeUserByUserEmail(string email);
        Task<DeleteUserResponse> DeleteUserByIdIfTokenHasSameAspNetUserId(int id, string jwtToken);
        Task<ApplicationUser> GetApplicationUserById(int id);
        Task<ApplicationUser> GetApplicationUserByEmail(string email);
        Task<ApplicationUser> GetApplicationUserTenantByEmail(string email);
    }
}
