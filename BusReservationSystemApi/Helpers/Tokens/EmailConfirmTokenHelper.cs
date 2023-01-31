using BusReservationSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BusReservationSystemApi.Helpers.Tokens
{
    public class EmailConfirmTokenHelper : TokenHelper, IEmailConfirmTokenHelper
    {
        private readonly UserManager<AppUser> _userManager;

        public EmailConfirmTokenHelper(UserManager<AppUser> userManager) : base(userManager)
        {
            _userManager = userManager;
        }

        public override async Task<string> GenerateToken(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = TokenEncoder(token);
            return encodedToken;
        }

        public async Task<bool> ConfirmEmailAction(AppUser user, string token)
        {
            bool flag = true;
            var decodedToken = TokenDecoder(token);
            var emailConfirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!emailConfirmResult.Succeeded)
            {
                flag = false;
            }
            return flag;
        }
    }

    public interface IEmailConfirmTokenHelper
    {
        Task<string> GenerateToken(AppUser user);
        Task<bool> ConfirmEmailAction(AppUser user, string token);
    }


}
