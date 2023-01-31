
using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BusReservationSystemApi.Helpers.Tokens
{
    public class PasswordResetTokenHelper : TokenHelper, IPasswordResetTokenHelper
    {
        private readonly UserManager<AppUser> _userManager;

        public PasswordResetTokenHelper(UserManager<AppUser> userManager) : base(userManager)
        {
            _userManager = userManager;
        }

        public override async Task<string> GenerateToken(AppUser user)
        {
            // FIXME: Either encrypt the password or remove this method.
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = TokenEncoder(token);
            return encodedToken;
        }

        public async Task<ServiceResponse<bool>> ResetPasswordAction(AppUser user, string token, string newPassword)
        {
            var resetPassResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!resetPassResult.Succeeded)
            {
                var serviceResponse = ServiceResponse<bool>.Failed(
                    "Failed to change password",
                    resetPassResult.Errors.Select(e => e.Description));
                return serviceResponse;
            }
            return ServiceResponse<bool>.Succeeded(resetPassResult.Succeeded, "Password changed successfully");
        }
    }

    public interface IPasswordResetTokenHelper
    {
        Task<string> GenerateToken(AppUser user);
        Task<ServiceResponse<bool>> ResetPasswordAction(AppUser user, string token, string newPassword);
    }

}

